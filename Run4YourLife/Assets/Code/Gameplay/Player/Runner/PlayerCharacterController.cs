using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;
using System;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(PlayerControlScheme))]
    public class PlayerCharacterController : MonoBehaviour, IEventMessageTarget
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

        [SerializeField]
        public float timeToIdle = 5.0f;

        #endregion

        #region References

        private CharacterController characterController;
        private Stats stats;
        private PlayerControlScheme playerControlScheme;
        private Animator anim;
        private Animation actualAnimation;

        #endregion

        #region Private Variables

        private bool m_isJumping;

        private Vector3 m_velocity;

        private bool facingRight = true;

        private float burnedHorizontal = 1.0f;

        private float idleTimer = 0.0f;

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
         
            if (!stats.root)
            {
                if (characterController.isGrounded && playerControlScheme.jump.Started())
                {
                    Jump();
                }

                Move();
            }
            else
            {
                if(playerControlScheme.interact.Started())
                {
                    stats.rootHardness -= 1;
                }

                if (stats.rootHardness == 0)
                {
                    stats.root = false;
                }
            }
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

            horizontal = CheckStatModificators(horizontal);

            Vector3 move = transform.forward * horizontal * stats.Get(StatType.SPEED) * Time.deltaTime;

            Vector3 speed = move + m_velocity * Time.deltaTime;

            anim.SetFloat("xSpeed", Mathf.Abs(speed.x));
            anim.SetFloat("timeToIdle", idleTimer);

            if ((facingRight && speed.x < 0) || (!facingRight && speed.x > 0))
            {
                Flip();
            }

            if(speed.x == 0)
            {
                idleTimer += Time.deltaTime;
            }
            else
            {
                idleTimer = 0.0f;
            }

            characterController.Move(move + m_velocity * Time.deltaTime);
        }

        private float CheckStatModificators(float controllerHorizontal)
        {
            float toReturn = controllerHorizontal;

            if (stats.burned)
            {
                if (toReturn > 0.0f)
                {
                    toReturn = 1.0f;
                    burnedHorizontal = toReturn;
                }
                else if (toReturn < 0.0f)
                {
                    toReturn = -1.0f;
                    burnedHorizontal = toReturn;
                }
                else
                {
                    toReturn = burnedHorizontal;
                }
            }

            if (stats.windPush)
            {
                toReturn -= 0.7f;
            }

            return toReturn;
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

        public void Explosion()
        {
            Destroy(gameObject);
        }

        public void Root(int rootHardness)
        {
            anim.SetTrigger("root");
            anim.SetFloat("xSpeed", 0.0f);

            stats.root = true;
            stats.rootHardness = rootHardness;
        }

        public void Impulse(Vector3 force)
        {
            Debug.Log("IMPULSE");
        }

        public void Debuff(StatModifier statmodifier)
        {
            stats.AddModifier(statmodifier);
        }

        public void Burned(int burnedTime)
        {
            if (!stats.burned)
            {
                StartCoroutine(BurnedCharacter(burnedTime));
            }
        }

        public void ActivateWindPush()
        {
            stats.windPush = true;
        }

        public void DeactivateWindPush()
        {
            stats.windPush = false;
        }

        private IEnumerator BurnedCharacter(int value)
        {
            stats.burned = true;
            yield return new WaitForSeconds(value);
            stats.burned = false;
        }
    }
}
