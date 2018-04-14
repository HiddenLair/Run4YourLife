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
    [RequireComponent(typeof(Stats))]
    [RequireComponent(typeof(Animator))]
    public class RunnerCharacterController : MonoBehaviour
    {
        #region InspectorVariables

        [SerializeField]
        private GameObject m_graphics;

        [SerializeField]
        private float m_coyoteGroundedTime;

        [SerializeField]
        private float m_baseHorizontalDrag;

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
        private float m_timeToIdle;

        #endregion

        #region Public Variable

        public AudioClip jumpClip;
        public AudioClip bounceClip;

        #endregion

        #region References

        private CharacterController m_characterController;
        private Stats m_stats;
        private RunnerControlScheme m_playerControlScheme;
        private Animator m_animator;
        private Animation m_currentAnimation;
        private AudioSource m_audioSource;
        private RunnerInputStated inputPlayer;

        #endregion

        #region Private Variables

        private bool m_isGroundedOrCoyoteGrounded;
        private bool m_isJumping;
        private bool m_isBouncing;
        private bool m_isBeingImpulsed;
        private bool m_ceilingCollision;
        private bool m_isFacingRight = true;

        private Vector3 m_velocity;
        private float m_gravity;
        private float m_horizontalDrag;

        private float m_idleTimer;

        private Coroutine m_checkCoyoteGroundedCoroutine;

        #endregion

        #region Attributes
        public Vector3 Velocity
        {
            get
            {
                return m_velocity;
            }
        }
        #endregion

        void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_playerControlScheme = GetComponent<RunnerControlScheme>();
            m_characterController = GetComponent<CharacterController>();
            m_stats = GetComponent<Stats>();
            m_animator = GetComponent<Animator>();
            m_gravity = m_baseGravity;
            m_horizontalDrag = m_baseHorizontalDrag;
            inputPlayer = GetComponent<RunnerInputStated>();
        }

        private void OnEnable()
        {
            m_checkCoyoteGroundedCoroutine = StartCoroutine(CheckCoyoteGroundedCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(m_checkCoyoteGroundedCoroutine);
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
                        yield return new WaitForSeconds(m_coyoteGroundedTime);
                        m_isGroundedOrCoyoteGrounded = false;
                    }
                }
                yield return null;
            }
        }

        void Update()
        {
            if (!m_isBeingImpulsed)
            {
                GravityAndDrag();


                if (m_isGroundedOrCoyoteGrounded && inputPlayer.GetJumpInput())
                {
                    Jump();
                }

                Move();
            }
            m_animator.SetBool("ground", m_characterController.isGrounded);
        }

        private void GravityAndDrag()
        {
            m_velocity.y += m_gravity * Time.deltaTime;

            m_velocity.x = Mathf.Lerp(m_velocity.x, 0.0f, m_horizontalDrag * Time.deltaTime);
            if(Mathf.Abs(m_velocity.x) < 1.0f)
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
            float horizontal = inputPlayer.GetHorizontalInput();

            Vector3 inputMovement = transform.right * horizontal * m_stats.Get(StatType.SPEED) * Time.deltaTime;
            Vector3 totalMovement = inputMovement + m_velocity * Time.deltaTime;

            MoveCharacterContoller(totalMovement);
        }

        private void MoveCharacterContoller(Vector3 movement)
        {
            m_characterController.Move(movement);
            TrimPlayerPositionInsideCameraView();

            m_animator.SetFloat("xSpeed", Mathf.Abs(movement.x));
            LookAtMovingSide();
            UpdateIdleTimer(movement);
        }

        private void TrimPlayerPositionInsideCameraView()
        {
            float xScreenRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Math.Abs(Camera.main.transform.position.z - transform.position.z))).x;
            if (transform.position.x > xScreenRight)
            {
                Vector3 trimmedPosition = transform.position;
                trimmedPosition.x = xScreenRight;
                transform.position = trimmedPosition;
            }
        }

        private void LookAtMovingSide()
        {
            bool shouldFaceTheOtherWay = (m_isFacingRight && m_playerControlScheme.move.Value() < 0) || (!m_isFacingRight && m_playerControlScheme.move.Value() > 0);
            if (shouldFaceTheOtherWay)
            {
                m_graphics.transform.Rotate(Vector3.up, 180);
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
            return Mathf.Sign(dragDistance)*Mathf.Sqrt(Mathf.Abs(dragDistance) * 2f * m_baseHorizontalDrag);
        }

        private IEnumerator JumpCoroutine()
        {
            m_isJumping = true;

            m_audioSource.PlayOneShot(jumpClip);
            m_animator.SetTrigger("jump");

            //set vertical velocity to the velocity needed to reach maxJumpHeight
            m_velocity.y = HeightToVelocity(m_stats.Get(StatType.JUMP_HEIGHT));
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
            yield return new WaitUntil(() => Time.time >= endTime || m_playerControlScheme.jump.Ended() || m_characterController.isGrounded || m_isBouncing);

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

            while (m_playerControlScheme.jump.Persists() && previousPositionY < transform.position.y && !m_ceilingCollision)
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

        public void BounceOnMe()
        {
            m_animator.SetTrigger("bump");
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
            Debug.Log("X: "+ m_velocity.x + " Y: "+ m_velocity.y);

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

            yield return new WaitForSeconds(0.5f);

            m_horizontalDrag = m_baseHorizontalDrag;
            m_isBeingImpulsed = false;
        }

        #endregion
    }
}
