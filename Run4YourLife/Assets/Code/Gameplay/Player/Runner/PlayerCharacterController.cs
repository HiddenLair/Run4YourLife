using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(PlayerControlScheme))]
    public class PlayerCharacterController : MonoBehaviour
    {

        #region InspectorVariables

        // [SerializeField]
        // private float m_speed;

        [SerializeField]
        private GameObject graphics;

        [SerializeField]
        private float m_gravity;

        [SerializeField]
        private float m_endOfJumpGravity;

        // [SerializeField]
        // private float m_jumpHeight;

        [SerializeField]
        private float m_jumpOnTopOfAnotherPlayerHeight;

        #endregion

        #region References


        private CharacterController characterController;
        private Stats stats;
        private PlayerControlScheme playerControlScheme;
        private Animator anim;

        #endregion

        #region Private Variables

        private bool m_isJumping;

        private Vector3 m_velocity;

        private bool facingRight = true;

        #endregion

        void Awake()
        {
            playerControlScheme = GetComponent<PlayerControlScheme>();
            characterController = GetComponent<CharacterController>();
            stats = GetComponent<Stats>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            playerControlScheme.Active = true;
        }

        void Update()
        {
            Gravity();

            anim.SetBool("ground", characterController.isGrounded);

            if (characterController.isGrounded && playerControlScheme.jump.Started())
            {
                Jump();
            }

            Move();
        }

        private void Gravity()
        {
            m_velocity.y += m_gravity * Time.deltaTime;

            if (!m_isJumping && characterController.isGrounded)
            {
                m_velocity.y = m_gravity * Time.deltaTime;
            }
        }

        private void Move()
        {
            float horizontal = playerControlScheme.move.Value();
            Vector3 move = transform.forward * horizontal * stats.Get(StatType.SPEED) * Time.deltaTime;

            Vector3 speed = move + m_velocity * Time.deltaTime;

            anim.SetFloat("xSpeed", Mathf.Abs(speed.x));
            if ((facingRight && speed.x < 0) || (!facingRight && speed.x > 0))
            {
                Flip();
            }

            characterController.Move(move + m_velocity * Time.deltaTime);
        }

        private void Jump()
        {
            StartCoroutine(JumpCoroutine());
        }

        #region Jump Coroutines

        private float HeightToVelocity(float height)
        {
            return Mathf.Sqrt(height * -2f * m_gravity);
        }

        private IEnumerator JumpCoroutine()
        {
            m_isJumping = true;
            anim.SetTrigger("jump");

            //set vertical velocity to the velocity needed to reach maxJumpHeight
            AddVelocity(new Vector3(0, HeightToVelocity(stats.Get(StatType.JUMP_HEIGHT)), 0));

            yield return StartCoroutine(WaitUntilApexOfJumpOrReleaseButton());

            m_isJumping = false;

            yield return StartCoroutine(FallFaster());
        }

        private IEnumerator FallFaster()
        {
            while (!characterController.isGrounded)
            {
                m_velocity.y += m_endOfJumpGravity * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator WaitUntilApexOfJumpOrReleaseButton()
        {
            float previousPositionY = transform.position.y;
            yield return null;

            while (playerControlScheme.jump.Persists() && previousPositionY < transform.position.y)
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

        internal void OnPlayerHasBeenJumpedOnTopByAnotherPlayer()
        {
            Debug.Log("Jumped on top");
        }

        internal void OnPlayerHasJumpedOnTopOfAnotherPlayer()
        {
            //TODO: Stop current jump
            m_velocity.y = HeightToVelocity(m_jumpOnTopOfAnotherPlayerHeight);
            anim.SetTrigger("bump");
        }

        private void Flip()
        {
            graphics.transform.Rotate(Vector3.up, 180);
            facingRight = !facingRight;
        }
    }
}
