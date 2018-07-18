using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using MonsterLove.StateMachine;

using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.InputManagement;
using Run4YourLife.Utils;
using Run4YourLife.CameraUtils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerControlScheme))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(RunnerAttributeController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(RunnerBounceController))]
    public class RunnerController : MonoBehaviour, IRunnerEvents
    {
        private enum States
        {
            Idle,
            Move,
            CoyoteMove,
            Jump,
            SecondJump,
            JumpSpeedReduction,
            JumpHover,
            Fall,
            Bounce,
            Dash,
            Push,
            Shock
        }

        #region InspectorVariables

        [SerializeField]
        private AudioClip m_dashClip;

        [SerializeField]
        private AudioClip m_jumpClip;

        [SerializeField]
        private AudioClip m_bounceClip;

        [SerializeField]
        private AudioClip m_fartClip;

        [SerializeField]
        private AudioClip m_deathClip;

        [SerializeField]
        private float m_coyoteGroundedTime;

        [SerializeField]
        private float m_impulseHorizontalDrag;

        [SerializeField]
        private float m_baseGravity;

        [SerializeField]
        private float m_hoverGravity;

        [SerializeField]
        private float m_hoverDuration;

        [SerializeField]
        private float m_releaseJumpButtonVelocityReductor;

        [SerializeField]
        private float m_secondJumpHeight;

        [SerializeField]
        private float m_endOfJumpGravity;

        [SerializeField]
        private float m_dashCooldown;

        [SerializeField]
        private float m_dashDistance;

        [SerializeField]
        private float m_dashTime;

        [SerializeField]
        private float m_timeToIdle;

        [SerializeField]
        private FXReceiver m_fartParticleReceiver;

        [SerializeField]
        private FXReceiver m_dustParticleReceiver;

        [SerializeField]
        private FXReceiver deathReceiver;

        [SerializeField]
        private float inmuneTimeAfterRevive = 2.0f;

        [SerializeField]
        private float fadePeriodAfterRevive = 0.2f;

        [SerializeField]
        private Renderer[] m_renderersToFade;

        #endregion

        #region References

        private CharacterController m_characterController;
        private RunnerAttributeController m_runnerAttributeController;
        private RunnerControlScheme m_runnerControlScheme;
        private RunnerBounceController m_runnerBounceController;
        private Animator m_animator;
        private InputController m_inputController;

        private Transform m_graphics;
        private Transform m_dashTrail;

        private Camera m_mainCamera;

        #endregion

        #region Private Members

        private StateMachine<States> m_stateMachine;

        private bool m_isReadyToDash = true;
        private bool m_ceilingCollision;
        private bool m_isFacingRight = true;

        private Vector3 m_velocity;
        private float m_gravity;
        private float m_horizontalDrag;

        private bool m_checkOutOfScreen;

        private bool m_canDoubleJumpAgain;

        private readonly float m_baseHorizontalDrag = 2f; // Hard coded because DragToVelocity calculates with a magic number

        private bool m_isShielded;

        private bool m_reviveMode;

        private bool m_recentlyRevived;

        private GameObject m_shieldGameObject;
        private MaterialFadeOut m_shieldMaterialFadeOut;
        private MaterialFitRunnerColor materialFitRunnerColor;

        private Coroutine shieldCooldownDestroy;
        private RunnerController m_runnerCharacterController;

        private float idleTimer = 0.0f;



        #endregion

        #region Attributes
        public Vector3 Velocity
        {
            get
            {
                return m_velocity;
            }
        }

        public float WindForceRelative { get; set; }

        public Vector3 ExternalVelocity { get; set; }

        public bool CheckOutScreen { get { return m_checkOutOfScreen; } set { m_checkOutOfScreen = value; } }

        #endregion

        private bool IsDashing { get { return m_stateMachine.State == States.Dash; } }


        private void Awake()
        {
            m_stateMachine = StateMachine<States>.Initialize(this);
            //m_stateMachine.Changed += (state) => Debug.Log(state); // Debug statement for characters

            m_runnerControlScheme = GetComponent<RunnerControlScheme>();
            m_characterController = GetComponent<CharacterController>();
            m_runnerAttributeController = GetComponent<RunnerAttributeController>();
            m_runnerBounceController = GetComponent<RunnerBounceController>();
            m_animator = GetComponent<Animator>();
            m_inputController = GetComponent<InputController>();

            m_graphics = transform.Find("Graphics");
            Debug.Assert(m_graphics != null);

            m_dashTrail = transform.Find("Graphics/DashTrail");
            Debug.Assert(m_dashTrail != null);

            m_mainCamera = CameraManager.Instance.MainCamera;
            Debug.Assert(m_mainCamera != null);

            m_gravity = m_baseGravity;
            m_horizontalDrag = m_baseHorizontalDrag;

            m_runnerCharacterController = GetComponent<RunnerController>();

            Transform shieldTransform = transform.Find("Graphics/Shield");
            Debug.Assert(shieldTransform != null);
            m_shieldGameObject = shieldTransform.gameObject;
            m_shieldMaterialFadeOut = m_shieldGameObject.GetComponent<MaterialFadeOut>();
            materialFitRunnerColor = m_shieldGameObject.GetComponent<MaterialFitRunnerColor>();
        }

        private void OnEnable()
        {
            ResetMembers();
            m_runnerControlScheme.Active = true;

            m_stateMachine.ChangeState(States.Idle);
        }

        private void ResetMembers()
        {
            m_gravity = m_baseGravity;
            m_horizontalDrag = m_baseHorizontalDrag;
            m_velocity = Vector3.zero;

            m_isReadyToDash = true;
            m_ceilingCollision = false;
            m_dashTrail.gameObject.SetActive(false);
            WindForceRelative = 0.0f;
            idleTimer = 0.0f;
            m_animator.Rebind();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            m_runnerControlScheme.Active = false;
        }

        private void Update()
        {
            if (m_isReadyToDash && m_inputController.Started(m_runnerControlScheme.Dash))
            {
                m_stateMachine.ChangeState(States.Dash);
            }
            //Idle time  manage
            idleTimer += Time.deltaTime;
        }

        #region Collision

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.activeSelf)
            {
                if (m_stateMachine.State == States.Jump && RunnerHitItsHead(hit))
                {
                    OnRunnerHitItsHead();
                }

                if (m_stateMachine.State != States.Jump && m_characterController.isGrounded)
                {
                    m_velocity.y = 0.0f;
                }

                if (IsDashing)
                {
                    IRunnerDashBreakable breakable = hit.gameObject.GetComponent<IRunnerDashBreakable>();
                    if (breakable != null)
                    {
                        breakable.Break();
                    }
                }
            }
        }

        private bool RunnerHitItsHead(ControllerColliderHit hit)
        {
            Vector3 hitVector = hit.point - transform.position;
            return hitVector.y > 0.3f && hitVector.x == 0;
        }

        private void OnRunnerHitItsHead()
        {
            m_ceilingCollision = true;
            m_velocity.y = 0;
        }

        #endregion

        #region Movement Utilities

        private void GravityAndDrag()
        {
            Gravity();
            Drag();
        }

        private void Gravity()
        {
            m_velocity.y += m_gravity * Time.deltaTime;
        }

        private void Drag()
        {
            m_velocity.x = m_velocity.x * (1 - m_horizontalDrag * Time.deltaTime);
            if (Mathf.Abs(m_velocity.x) < 1.0f)
            {
                m_velocity.x = 0;
            }
        }

        private void Move()
        {
            float horizontal = m_inputController.ValueMaximized(m_runnerControlScheme.Move);

            if(horizontal != 0)
            {
                idleTimer = 0.0f;//Reset idle timer
            }

            Vector3 windMovement = (Vector3.left * m_runnerAttributeController.GetAttribute(RunnerAttribute.Speed) * Time.deltaTime) * WindForceRelative;
            Vector3 inputMovement = transform.right * (horizontal * m_runnerAttributeController.GetAttribute(RunnerAttribute.Speed) * Time.deltaTime);
            Vector3 totalMovement = inputMovement + (m_velocity + windMovement) * Time.deltaTime + ExternalVelocity * Time.deltaTime;

            ExternalVelocity = Vector3.zero;

            Move(totalMovement);
        }

        private void Move(Vector3 movement)
        {
            MoveCharacterContoller(movement);
            LookAtMovingSide();
        }

        private void MoveCharacterContoller(Vector3 movement)
        {
            m_characterController.Move(movement);
            if (m_checkOutOfScreen)
            {
                TrimPlayerPositionHorizontalInsideGameplayArea();
            }

            m_animator.SetFloat(RunnerAnimation.xSpeed, Mathf.Abs(movement.x));
            m_animator.SetFloat(RunnerAnimation.ySpeed, m_velocity.y);
            m_animator.SetBool(RunnerAnimation.isGrounded, m_characterController.isGrounded);
        }

        private void TrimPlayerPositionHorizontalInsideGameplayArea()
        {
            float xScreenLeft = CameraConverter.ViewportToGamePlaneWorldPosition(m_mainCamera, new Vector2(0, 0)).x;
            float xScreenRight = CameraConverter.ViewportToGamePlaneWorldPosition(m_mainCamera, new Vector2(1, 1)).x;

            Vector3 trimmedPosition = new Vector3()
            {
                x = Mathf.Clamp(transform.position.x, xScreenLeft, xScreenRight),
                y = transform.position.y,
                z = 0
            };
            transform.position = trimmedPosition;
        }

        private void LookAtMovingSide()
        {
            bool shouldFaceTheOtherWay = (m_isFacingRight && m_runnerControlScheme.Move.ValueMaximized() < 0) || (!m_isFacingRight && m_runnerControlScheme.Move.ValueMaximized() > 0);
            if (shouldFaceTheOtherWay)
            {
                m_isFacingRight = !m_isFacingRight;
                RotateGraphicsAtFacingDirection();
            }
        }

        private void RotateGraphicsAtFacingDirection()
        {
            float sign = m_isFacingRight ? -1 : 1;
            m_graphics.rotation = Quaternion.Euler(0, sign * 90, 0);
        }

        #endregion

        #region Idle

        private void Idle_Enter()
        {
            m_canDoubleJumpAgain = true;
            m_animator.SetTrigger(RunnerAnimation.idle);
        }

        private void Idle_Update()
        {
            GravityAndDrag();
            Move();

            if (m_inputController.Started(m_runnerControlScheme.Jump))
            {
                m_stateMachine.ChangeState(States.Jump);
            }
            else if (m_characterController.isGrounded)
            {
                if (m_runnerControlScheme.Move.ValueMaximized() != 0)
                {
                    m_dustParticleReceiver.PlayFx(false);
                    m_stateMachine.ChangeState(States.Move);
                }
                else if (m_velocity.sqrMagnitude != 0)
                {
                    m_stateMachine.ChangeState(States.Move);
                }
            }
            else
            { // !m_characterController.isGrounded
                m_stateMachine.ChangeState(States.Fall);
            }
        }

        #endregion

        #region Move

        private void Move_Enter()
        {
            m_canDoubleJumpAgain = true;
        }

        private void Move_Update()
        {
            GravityAndDrag();
            Move();

            if (m_inputController.Started(m_runnerControlScheme.Jump))
            {
                m_stateMachine.ChangeState(States.Jump);
            }
            else if (!m_characterController.isGrounded)
            {
                m_stateMachine.ChangeState(States.CoyoteMove);
            }
            else if (m_runnerControlScheme.Move.ValueMaximized() == 0 && m_velocity == Vector3.zero && idleTimer >= m_timeToIdle)
            {
                m_stateMachine.ChangeState(States.Idle);
            }
        }

        #endregion

        #region CoyoteMove

        private float m_coyoteMove_endTime;

        private void CoyoteMove_Enter()
        {
            m_coyoteMove_endTime = Time.time + m_coyoteGroundedTime;
        }

        private void CoyoteMove_Update()
        {
            GravityAndDrag();
            Move();

            if (m_inputController.Started(m_runnerControlScheme.Jump))
            {
                m_stateMachine.ChangeState(States.Jump);
            }
            else if (m_characterController.isGrounded)
            {
                m_stateMachine.ChangeState(States.Move);
            }
            else if (Time.time >= m_coyoteMove_endTime)
            {
                m_stateMachine.ChangeState(States.Fall);
            }
        }

        #endregion

        #region Jump

        private float m_jump_previousPositionY;

        private float HeightToVelocity(float height)
        {
            return Mathf.Sqrt(height * -2.0f * m_baseGravity);
        }

        private float VelocityToHeight(float velocity)
        {
            return Mathf.Sqrt(velocity / (-2.0f * m_baseGravity));
        }

        private float DragToVelocity(float dragDistance)
        {
            return 2.1129f * dragDistance;
        }

        private void Jump_Enter()
        {
            AudioManager.Instance.PlaySFX(m_jumpClip);
            m_animator.SetTrigger(RunnerAnimation.jump);

            //set vertical velocity to the velocity needed to reach maxJumpHeight
            m_velocity.y = HeightToVelocity(m_runnerAttributeController.GetAttribute(RunnerAttribute.JumpHeight));
            m_jump_previousPositionY = transform.position.y;
        }
        private void Jump_Update()
        {
            GravityAndDrag();
            Move();

            if (m_ceilingCollision || m_runnerControlScheme.Jump.Ended() || m_jump_previousPositionY >= transform.position.y)
            {
                //Transitions to next states
                if (m_runnerControlScheme.Jump.Persists() && !m_ceilingCollision)
                {
                    m_stateMachine.ChangeState(States.JumpSpeedReduction);
                }
                else
                {
                    m_ceilingCollision = false;

                    m_stateMachine.ChangeState(States.Fall);
                }
            }

            m_jump_previousPositionY = transform.position.y;
        }

        #endregion

        #region SecondJump

        private float m_secondJump_previousYPosition;

        private void SecondJump_Enter()
        {
            m_canDoubleJumpAgain = false;

            AudioManager.Instance.PlaySFX(m_fartClip);
            m_fartParticleReceiver.PlayFx();
            m_animator.SetTrigger(RunnerAnimation.jump);

            float jumpHeight = m_secondJumpHeight;
            if (m_velocity.y > 0)
            {
                jumpHeight += VelocityToHeight(m_velocity.y);
            }
            m_velocity.y = HeightToVelocity(jumpHeight);

            m_secondJump_previousYPosition = transform.position.y;
        }

        private void SecondJump_Update()
        {
            GravityAndDrag();
            Move();

            if (m_ceilingCollision || m_runnerControlScheme.Jump.Ended() || m_secondJump_previousYPosition >= transform.position.y)
            {
                //Transitions to next states
                if (m_runnerControlScheme.Jump.Persists() && !m_ceilingCollision)
                {
                    m_stateMachine.ChangeState(States.JumpSpeedReduction);
                }
                else
                {
                    m_ceilingCollision = false;

                    m_stateMachine.ChangeState(States.Fall);
                }
            }

            m_secondJump_previousYPosition = transform.position.y;
        }

        #endregion

        #region JumpSpeedReduction

        private void JumpSpeedReduction_Update()
        {
            m_velocity.y = Mathf.Lerp(m_velocity.y, 0.0f, m_releaseJumpButtonVelocityReductor * Time.deltaTime);

            GravityAndDrag();
            Move();

            if (m_velocity.y <= 0f || m_runnerControlScheme.Jump.Ended())
            {
                m_stateMachine.ChangeState(States.JumpHover);
            }
        }

        #endregion

        #region JumpHover

        private float m_secondJumpHover_EndTimer;

        void JumpHover_Enter()
        {
            m_gravity = m_hoverGravity;
            m_secondJumpHover_EndTimer = Time.time + m_hoverDuration;
        }

        private void JumpHover_Exit()
        {
            m_gravity = m_baseGravity;
        }

        private void JumpHover_Update()
        {
            GravityAndDrag();
            Move();

            if (Time.time >= m_secondJumpHover_EndTimer || m_runnerControlScheme.Jump.Ended())
            {
                m_stateMachine.ChangeState(States.Fall);
            }
        }

        #endregion

        #region Fall

        private void Fall_Enter()
        {
            m_gravity = m_endOfJumpGravity;
        }

        private void Fall_Exit()
        {
            m_gravity = m_baseGravity;
        }

        private void Fall_Update()
        {
            GravityAndDrag();
            Move();

            if (m_runnerControlScheme.Jump.Started())
            {
                bool didExecuteBounce = m_runnerBounceController.ExecuteBounceIfPossible();
                if (!didExecuteBounce && m_canDoubleJumpAgain)
                {
                    m_stateMachine.ChangeState(States.SecondJump);
                }
            }
            else if (m_characterController.isGrounded)
            {
                m_stateMachine.ChangeState(States.Move);
            }
        }

        #endregion

        #region Bounce

        private Vector3 _bounce_bounceForce;

        private float m_bounce_previousY;

        public void Bounce(Vector3 bounceForce)
        {
            _bounce_bounceForce = bounceForce;

            m_stateMachine.ChangeState(States.Bounce);
        }

        private IEnumerator Bounce_Enter()
        {
            m_canDoubleJumpAgain = true;

            AudioManager.Instance.PlaySFX(m_bounceClip);

            m_velocity.x = DragToVelocity(_bounce_bounceForce.x);
            m_velocity.y = HeightToVelocity(_bounce_bounceForce.y);

            m_bounce_previousY = transform.position.y;
            // bounce may have been cause by a jump we have to wait one frame 
            //so that it does not detect the same input as a second jump
            yield return null;
        }

        private void Bounce_Update()
        {
            GravityAndDrag();
            Move();

            if (m_runnerControlScheme.Jump.Started())
            {
                m_stateMachine.ChangeState(States.SecondJump);
            }
            else if (m_bounce_previousY >= transform.position.y)
            {
                m_stateMachine.ChangeState(States.Fall);
            }

            m_bounce_previousY = transform.position.y;
        }

        #endregion

        #region Dash

        private float m_dash_endTime;

        private void Dash_Enter()
        {
            AudioManager.Instance.PlaySFX(m_dashClip);

            m_isReadyToDash = false;
            m_animator.SetTrigger(RunnerAnimation.dash);

            m_dashTrail.gameObject.SetActive(true);

            float sign = m_isFacingRight ? 1 : -1;
            m_velocity.x = sign * m_dashDistance / m_dashTime;
            m_velocity.y = 0.0f;

            m_dash_endTime = Time.time + m_dashTime;
        }

        void Dash_Exit()
        {
            m_velocity.x = 0;
            m_dashTrail.gameObject.SetActive(false);
        }

        private void Dash_Update()
        {
            Move(m_velocity * Time.deltaTime);
            if (Time.time >= m_dash_endTime)
            {
                StartCoroutine(YieldHelper.WaitForSeconds(() => m_isReadyToDash = true, m_dashCooldown)); // set ready to dash after some time

                if (m_characterController.isGrounded)
                {
                    m_stateMachine.ChangeState(States.Move);
                }
                else
                {
                    m_stateMachine.ChangeState(States.Fall);
                }
            }
        }

        #endregion

        #region Push

        private Vector3 m_push_force;

        public void Push(Vector3 force)
        {
            m_push_force = force;
            m_stateMachine.ChangeState(States.Push);
        }

        private void Push_Enter()
        {
            m_horizontalDrag = m_impulseHorizontalDrag;

            m_animator.SetTrigger(RunnerAnimation.push);
            m_animator.SetFloat(RunnerAnimation.pushForce, m_push_force.x);

            m_velocity += m_push_force;
        }

        private void Push_Exit()
        {
            m_horizontalDrag = m_baseHorizontalDrag;
        }

        private void Push_Update()
        {
            GravityAndDrag();
            Move(m_velocity * Time.deltaTime);

            if (m_velocity.x == 0f)
            {
                m_stateMachine.ChangeState(States.Move);
            }
        }

        #endregion

        #region Shock

        private float m_shock_endTime;

        public void Shock(float time)
        {
            m_shock_endTime = Time.time + time;
            m_stateMachine.ChangeState(States.Shock);
        }

        private void Shock_Enter()
        {
            m_velocity.x = 0;
            m_graphics.rotation = Quaternion.identity;
            //Activate particle effects
            //Activate shock animation
            m_animator.SetTrigger(RunnerAnimation.shock);
        }

        private void Shock_Exit()
        {
            RotateGraphicsAtFacingDirection();
            //Deactivate particle effects
            //Deactivate animation
            m_animator.SetTrigger(RunnerAnimation.stopShock);
        }

        private void Shock_Update()
        {
            GravityAndDrag();
            MoveCharacterContoller(m_velocity * Time.deltaTime);

            if (Time.time >= m_shock_endTime)
            {
                m_stateMachine.ChangeState(States.Idle);
            }
        }

        #endregion

        #region Shield

        public void ActivateShield(float shieldTime)
        {
            m_isShielded = true;
            m_shieldGameObject.SetActive(true);
            m_shieldMaterialFadeOut.Activate(shieldTime);
            materialFitRunnerColor.Activate(GetComponent<PlayerInstance>().PlayerHandle.CharacterType);
        }

        public void DeactivateShield()
        {
            m_isShielded = false;
            m_shieldGameObject.SetActive(false);
        }

        /// <summary>
        /// Consumes shield if the shield is active
        /// </summary>
        /// <returns>True when it had a shield, false when it did not have a shield</returns>
        public bool ConsumeShieldIfAviable()
        {
            if (m_isShielded)
            {
                Destroy(GetComponent<Shielded>()); // this will call deactivate shield
                return true;
            }

            return false;
        }

        #endregion

        #region Character Effects

        public void Kill()
        {
            if (!ConsumeShieldIfAviable() && !m_recentlyRevived)
            {
                AudioManager.Instance.PlaySFX(m_deathClip);
                deathReceiver.PlayFx();
                GameplayPlayerManager.Instance.OnRunnerDeath(GetComponent<PlayerInstance>().PlayerHandle, transform.position);
            }
        }

        public void AbsoluteKill()
        {
            AudioManager.Instance.PlaySFX(m_deathClip);

            ConsumeShieldIfAviable();
            deathReceiver.PlayFx();
            GameplayPlayerManager.Instance.OnRunnerDeath(GetComponent<PlayerInstance>().PlayerHandle, transform.position);
        }

        public void Impulse(Vector3 force)
        {
            if (!ConsumeShieldIfAviable() && !m_recentlyRevived)
            {
                m_runnerCharacterController.Push(force);
            }
        }

        public void RecentlyRevived()
        {
            m_recentlyRevived = true;
            StartCoroutine(ManageRecentlyRevived());
        }

        IEnumerator ManageRecentlyRevived()
        {
            bool faded = false;
            float timer = Time.time + inmuneTimeAfterRevive;
            float fadeTimer = Time.time + fadePeriodAfterRevive;
            while (timer > Time.time)
            {
                if (fadeTimer <= Time.time)
                {
                    FadeByRender(faded);
                    faded = !faded;
                    fadeTimer = Time.time + fadePeriodAfterRevive;
                }
                yield return null;
            }
            FadeByRender(true);
            m_recentlyRevived = false;
        }

        private void FadeByRender(bool active)
        {
            foreach (Renderer renderer in m_renderersToFade)
            {
                renderer.enabled = active;
            }
        }

        #endregion

        #region Setters
        public void SetReviveMode(bool value)
        {
            m_reviveMode = value;
        }
        #endregion

        #region Getters
        public bool GetReviveMode()
        {
            return m_reviveMode;
        }
        #endregion
    }
}
