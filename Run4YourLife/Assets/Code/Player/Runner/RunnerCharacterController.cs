using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;
using System;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(RunnerControlScheme))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(RunnerAttributeController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(BumpController))]
    public class RunnerCharacterController : MonoBehaviour
    {
        #region InspectorVariables

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
        private float m_endOfJumpGravity;

        [SerializeField]
        private float m_dashCooldown;

        [SerializeField]
        private float m_dashDistance;

        [SerializeField]
        private float m_dashHorizontalDrag;

        [SerializeField]
        private float m_timeToIdle;

        [SerializeField]
        private AudioClip jumpClip;

        [SerializeField]
        private AudioClip bounceClip;

        #endregion

        #region References

        private CharacterController m_characterController;
        private RunnerAttributeController m_runnerAttributeController;
        private RunnerControlScheme m_runnerControlScheme;
        private Animator m_animator;
        private AudioSource m_audioSource;
        private InputController m_inputController;
        private BumpController m_bumpController;

        private Transform m_graphics;
        private Transform m_dashTrail;

        private Camera m_mainCamera;

        #endregion

        #region Private Variables

        private WaitForSeconds coyoteDelay;
        private WaitForSeconds dashDelay;

        private bool m_isGroundedOrCoyoteGrounded;
        private bool m_isJumping;
        private bool m_isBouncing;
        private bool m_isBeingImpulsed;
        private bool m_isDashing;
        private bool m_isReadyToDash = true;
        private bool m_ceilingCollision;
        private bool m_isFacingRight = true;

        private Vector3 m_velocity;
        private float m_gravity;
        private float m_horizontalDrag;

        private bool m_checkOutOfScreen;

        private float m_idleTimer;

        private readonly float m_baseHorizontalDrag = 2f; // Hard coded because DragToVelocity calculates with a magic number

        #endregion

        #region Attributes
        public Vector3 Velocity
        {
            get
            {
                return m_velocity;
            }
        }

        public Vector3 ExternalVelocity { get; set; }

        public bool IsDashing { get { return m_isDashing; } }

        public bool CheckOutScreen { get { return m_checkOutOfScreen; } set { m_checkOutOfScreen = value; } }
        #endregion

        void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_runnerControlScheme = GetComponent<RunnerControlScheme>();
            m_characterController = GetComponent<CharacterController>();
            m_runnerAttributeController = GetComponent<RunnerAttributeController>();
            m_animator = GetComponent<Animator>();
            m_inputController = GetComponent<InputController>();
            m_bumpController = GetComponent<BumpController>();

            m_graphics = transform.Find("Graphics");
            Debug.Assert(m_graphics != null);

            m_dashTrail = transform.Find("Graphics/DashTrail");
            Debug.Assert(m_dashTrail != null);

            m_mainCamera = CameraManager.Instance.MainCamera;
            Debug.Assert(m_mainCamera != null);

            m_gravity = m_baseGravity;
            m_horizontalDrag = m_baseHorizontalDrag;

            coyoteDelay = new WaitForSeconds(m_coyoteGroundedTime);
            dashDelay = new WaitForSeconds(m_dashCooldown);
        }

        private void OnEnable()
        {
            StartCoroutine(CheckCoyoteGroundedCoroutine());
            m_runnerControlScheme.Active = true;

            ResetMembers();
        }

        private void ResetMembers()
        {
            m_gravity = m_baseGravity;
            m_horizontalDrag = m_baseHorizontalDrag;
            m_velocity = Vector3.zero;

            m_isGroundedOrCoyoteGrounded = false;
            m_isJumping = false;
            m_isBouncing = false;
            m_isBeingImpulsed = false;
            m_isDashing = false;
            m_isReadyToDash = true;
            m_ceilingCollision = false;

            m_animator.Rebind();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            m_runnerControlScheme.Active = false;
        }

        private IEnumerator CheckCoyoteGroundedCoroutine()
        {
            while(true)
            {
                if (m_characterController.isGrounded)
                {
                    m_isGroundedOrCoyoteGrounded = true;
                }
                else
                {
                    if(m_isGroundedOrCoyoteGrounded)
                    {
                        yield return coyoteDelay;
                        m_isGroundedOrCoyoteGrounded = false;
                    }
                }
                yield return null;
            }
        }

        void Update()
        {
            if (!m_isBeingImpulsed && !m_isDashing)
            {
                GravityAndDrag();


                if (m_isGroundedOrCoyoteGrounded && m_inputController.Started(m_runnerControlScheme.Jump))
                {
                    Jump();
                }

                if (m_isReadyToDash && m_inputController.Started(m_runnerControlScheme.Dash))
                {
                    Dash();
                }

                Move();
            }
            m_animator.SetFloat("ySpeed",m_velocity.y);
            m_animator.SetBool("ground", m_characterController.isGrounded);
        }

        private void Dash()
        {
            StartCoroutine(DashCoroutine());
        }

        private IEnumerator DashCoroutine()
        {
            m_isDashing = true;
            m_isReadyToDash = false;
            m_animator.SetTrigger("dash");

            m_dashTrail.gameObject.SetActive(true);

            m_horizontalDrag = m_dashHorizontalDrag;
            float facingRight = m_isFacingRight ? 1 : -1;
            m_velocity.x = facingRight * DragToVelocity(m_dashDistance);
            m_velocity.y = 0.0f;
            while (m_velocity.x != 0)
            {
                Drag();
                MoveCharacterContoller(m_velocity * Time.deltaTime);
                yield return null;
            }

            m_horizontalDrag = m_baseHorizontalDrag;

            m_dashTrail.gameObject.SetActive(false);

            m_isDashing = false;
            yield return new WaitUntil(() => m_characterController.isGrounded);
            yield return dashDelay;
            m_isReadyToDash = true;
        }

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

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            
            if(m_isJumping && RunnerHitItsHead(hit))
            {
                OnRunnerHitItsHead();
            }

            if (!m_isJumping && m_characterController.isGrounded)
            {
                m_velocity.y = 0.0f;
            }
        }

        private bool RunnerHitItsHead(ControllerColliderHit hit)
        {
            Vector3 hitVector = hit.point - transform.position;
            return hitVector.y > 0.3f && hitVector.x == 0;
        }

        void OnRunnerHitItsHead()
        {
            m_ceilingCollision = true;
            m_velocity.y = 0;
        }

        private void Move()
        {
            float horizontal = m_inputController.Value(m_runnerControlScheme.Move);

            Vector3 inputMovement = transform.right * horizontal * m_runnerAttributeController.GetAttribute(RunnerAttribute.Speed) * Time.deltaTime;
            Vector3 totalMovement = inputMovement + m_velocity * Time.deltaTime + ExternalVelocity * Time.deltaTime;

            ExternalVelocity = Vector3.zero;

            MoveCharacterContoller(totalMovement);
        }

        private void MoveCharacterContoller(Vector3 movement)
        {
            m_characterController.Move(movement);
            if (m_checkOutOfScreen)
            {
                TrimPlayerPositionHorizontalInsideCameraView();
            }

            m_animator.SetFloat("xSpeed", Mathf.Abs(movement.x));
            LookAtMovingSide();
            UpdateIdleTimer(movement);
        }

        private void TrimPlayerPositionHorizontalInsideCameraView()
        {
            float xScreenLeft = m_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(m_mainCamera.transform.position.z - transform.position.z))).x;
            float xScreenRight = m_mainCamera.ScreenToWorldPoint(new Vector3(m_mainCamera.pixelWidth, 0, Math.Abs(m_mainCamera.transform.position.z - transform.position.z))).x;
            Vector3 trimmedPosition = transform.position;
            trimmedPosition.x = Mathf.Clamp(trimmedPosition.x, xScreenLeft, xScreenRight);
            transform.position = trimmedPosition;
        }

        private void LookAtMovingSide()
        {
            bool shouldFaceTheOtherWay = (m_isFacingRight && m_runnerControlScheme.Move.Value() < 0) || (!m_isFacingRight && m_runnerControlScheme.Move.Value() > 0);
            if (shouldFaceTheOtherWay)
            {
                m_graphics.Rotate(Vector3.up, 180);
                m_isFacingRight = !m_isFacingRight;
            }
        }

        private void UpdateIdleTimer(Vector3 totalMovement)
        {
            bool isMoving = totalMovement != Vector3.zero;
            m_idleTimer = isMoving ? m_idleTimer + Time.deltaTime : 0.0f;
            m_animator.SetFloat("timeToIdle", m_idleTimer);
        }

        #region Jump

        private void Jump()
        {
            m_isGroundedOrCoyoteGrounded = false;
            StartCoroutine(JumpCoroutine());
        }

        private float HeightToVelocity(float height)
        {
            return Mathf.Sqrt(height * -2f * m_baseGravity);
        }

        private float DragToVelocity(float dragDistance)
        {
            return 2.1129f * dragDistance;
        }

        private IEnumerator JumpCoroutine()
        {
            m_isJumping = true;

            m_audioSource.PlayOneShot(jumpClip);
            m_animator.SetTrigger("jump");

            //set vertical velocity to the velocity needed to reach maxJumpHeight
            m_velocity.y = HeightToVelocity(m_runnerAttributeController.GetAttribute(RunnerAttribute.JumpHeight));
            yield return StartCoroutine(WaitUntilApexOfJumpOrReleaseButtonOrCeiling());

            m_isJumping = false;

            if (!m_ceilingCollision)
            {
                yield return StartCoroutine(JumpHover());
            }
            else
            {
                m_ceilingCollision = false;
            }


            yield return StartCoroutine(FallFaster());
        }

        private IEnumerator JumpHover()
        {
            while(m_velocity.y > 0f)
            {
                yield return null;
                m_velocity.y = Mathf.Lerp(m_velocity.y, 0.0f, m_releaseJumpButtonVelocityReductor * Time.deltaTime);
            }

            m_gravity = m_hoverGravity;

            float endTime = Time.time + m_hoverDuration;
            yield return new WaitUntil(() => Time.time >= endTime || m_runnerControlScheme.Jump.Ended() || m_characterController.isGrounded || m_isBouncing);

            m_gravity = m_baseGravity;
        }

        private IEnumerator FallFaster()
        {
            m_gravity += m_endOfJumpGravity;
            yield return new WaitUntil(() => m_characterController.isGrounded || m_isBouncing);
            m_gravity -= m_endOfJumpGravity;
        }

        private IEnumerator WaitUntilApexOfJumpOrReleaseButtonOrCeiling()
        {
            float previousPositionY = transform.position.y;
            yield return null;

            while (m_runnerControlScheme.Jump.Persists() && previousPositionY < transform.position.y && !m_ceilingCollision)
            {                
                previousPositionY = transform.position.y;
                yield return null;
            }
        }

        #endregion

        public void AddVelocity(Vector3 velocity)
        {
            m_velocity += velocity;
        }

        #region Bounce

        public void BouncedOn()
        {
            m_bumpController.Bump();
        }

        public void Bounce(Vector3 bounceForce)
        {
            StartCoroutine(BounceCoroutine(bounceForce));
        }

        IEnumerator BounceCoroutine(Vector3 bounceForce)
        {
            m_isBouncing = true;
            m_audioSource.PlayOneShot(bounceClip);
            m_velocity.x = DragToVelocity(bounceForce.x);
            m_velocity.y = HeightToVelocity(bounceForce.y);

            yield return StartCoroutine(WaitUntilApexOfBounce());
            m_isBouncing = false;

            yield return StartCoroutine(FallFaster());
        }

        private IEnumerator WaitUntilApexOfBounce()
        {
            float previousPositionY = transform.position.y;
            yield return null;

            while (previousPositionY < transform.position.y)
            {
                previousPositionY = transform.position.y;
                yield return null;
            }
        }

        #endregion

        #region Impulse

        public void Impulse(Vector3 force)
        {
            StartCoroutine(ImpulseCoroutine(force));
        }

        IEnumerator ImpulseCoroutine(Vector3 force)
        {
            m_isBeingImpulsed = true;
            m_horizontalDrag = m_impulseHorizontalDrag;

            m_animator.SetTrigger("push");
            m_animator.SetFloat("pushForce", force.x);

            AddVelocity(force);

            while(m_velocity.x != 0.0f)
            {
                yield return null;
                GravityAndDrag();
                MoveCharacterContoller(m_velocity * Time.deltaTime);
            }

            float endOfMovement = Time.time + 0.5f;
            while (Time.time < endOfMovement)
            {
                yield return null;
                GravityAndDrag();
                MoveCharacterContoller(m_velocity * Time.deltaTime);
            }

            m_horizontalDrag = m_baseHorizontalDrag;
            m_isBeingImpulsed = false;
        }

        #endregion
    }
}
