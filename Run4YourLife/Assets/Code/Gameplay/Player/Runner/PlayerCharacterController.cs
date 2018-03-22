﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;
using System;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerControlScheme))]
    public class PlayerCharacterController : MonoBehaviour, ICharacterEvents
    {
        #region InspectorVariables

        [SerializeField]
        private GameObject graphics;

        [SerializeField]
        private float m_baseGravity;

        [SerializeField]
        private float m_endOfJumpGravity;

        [SerializeField]
        private float m_gravityPushMuliplier;

        [SerializeField]
        private float m_timeToIdle;

        [SerializeField]
        private float m_pushReduction;

        [SerializeField]
        private float m_minPushMagnitude;

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

        #endregion

        #region Private Variables

        private bool m_isJumping;
        private bool m_isBouncing;
        private bool m_isBeingImpulsed;

        private bool m_isFacingRight = true;

        private Vector3 m_velocity;
        private float m_gravity;

        private float m_burnedHorizontal = 1.0f;

        private float m_idleTimer = 0.0f;

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
        }

        private void Start()
        {
            m_playerControlScheme.Active = true;
        }

        void Update()
        {
            if (!m_isBeingImpulsed && !m_stats.root)
            {
                Gravity();

                if (m_characterController.isGrounded && m_playerControlScheme.jump.Started())
                {
                    Jump();
                }

                Move();

                m_animator.SetBool("ground", m_characterController.isGrounded);
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag(Tags.Interactable) && m_playerControlScheme.interact.Started())
            {
                ExecuteEvents.Execute<IPropEvents>(collider.gameObject, null, (x, y) => x.OnInteraction());
            }
        }

        private void Gravity()
        {
            m_velocity.y += m_gravity * Time.deltaTime;

            if (!m_isJumping && m_characterController.isGrounded)
            {
                m_velocity.y = m_gravity * Time.deltaTime;
            }
        }

        private void Move()
        {
            float horizontal = m_playerControlScheme.move.Value();

            horizontal = CheckStatModificators(horizontal);

            Vector3 move = transform.forward * horizontal * m_stats.Get(StatType.SPEED) * Time.deltaTime;

            Vector3 speed = move + m_velocity * Time.deltaTime;

            m_animator.SetFloat("xSpeed", Mathf.Abs(speed.x));
            m_animator.SetFloat("timeToIdle", m_idleTimer);

            if ((m_isFacingRight && speed.x < 0) || (!m_isFacingRight && speed.x > 0))
            {
                Flip();
            }

            if(speed.x == 0)
            {
                m_idleTimer += Time.deltaTime;
            }
            else
            {
                m_idleTimer = 0.0f;
            }

            MoveCharacterContoller(move + m_velocity * Time.deltaTime);
        }

        private void MoveCharacterContoller(Vector3 movement)
        {
            m_characterController.Move(movement);
            float xScreenRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Math.Abs(Camera.main.transform.position.z - transform.position.z))).x;
            if (transform.position.x > xScreenRight)
            {
                Vector3 tempPos = transform.position;
                tempPos.x = xScreenRight;
                transform.position = tempPos;
            }
        }

        private float CheckStatModificators(float controllerHorizontal)
        {
            float toReturn = controllerHorizontal;

            if (m_stats.burned)
            {
                if (toReturn > 0.0f)
                {
                    toReturn = 1.0f;
                    m_burnedHorizontal = toReturn;
                }
                else if (toReturn < 0.0f)
                {
                    toReturn = -1.0f;
                    m_burnedHorizontal = toReturn;
                }
                else
                {
                    toReturn = m_burnedHorizontal;
                }
            }

            if (m_stats.windPush)
            {
                toReturn -= 0.7f;
            }

            return toReturn;
        }

        #region Jump

        private void Jump()
        {
            StartCoroutine(JumpCoroutine());
        }

        private float HeightToVelocity(float height)
        {
            return Mathf.Sqrt(height * -2f * m_baseGravity);
        }

        private IEnumerator JumpCoroutine()
        {
            PlaySFX(jumpClip);
            m_isJumping = true;
            m_animator.SetTrigger("jump");

            //set vertical velocity to the velocity needed to reach maxJumpHeight
            m_velocity.y = HeightToVelocity(m_stats.Get(StatType.JUMP_HEIGHT));
            yield return StartCoroutine(WaitUntilApexOfJumpOrReleaseButton());

            m_isJumping = false;

            yield return StartCoroutine(FallFaster());
        }

        private IEnumerator FallFaster()
        {
            m_gravity += m_endOfJumpGravity;
            yield return new WaitUntil(() => m_characterController.isGrounded || m_isBouncing || m_isJumping);
            m_gravity -= m_endOfJumpGravity;
        }

        private IEnumerator WaitUntilApexOfJumpOrReleaseButton()
        {
            float previousPositionY = transform.position.y;
            yield return null;

            while (m_playerControlScheme.jump.Persists() && previousPositionY < transform.position.y)
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

        internal void BounceOnMe()
        {
            m_animator.SetTrigger("bump");
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
            {
                m_audioSource.PlayOneShot(clip);
            }
        }

        #region Bounce

        public void Bounce(float bounceForce)
        {
            StartCoroutine(BounceCoroutine(bounceForce));
        }

        IEnumerator BounceCoroutine(float bounceForce)
        {
            m_isBouncing = true;
            PlaySFX(bounceClip);
            m_velocity.y = HeightToVelocity(bounceForce);

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

        private void Flip()
        {
            graphics.transform.Rotate(Vector3.up, 180);
            m_isFacingRight = !m_isFacingRight;
        }

        public void Kill()
        {
            GameObject playerStateManager = FindObjectOfType<PlayerStateManager>().gameObject;
            PlayerDefinition playerDefinition = GetComponent<PlayerInstance>().PlayerDefinition;
            ExecuteEvents.Execute<IPlayerStateEvents>(playerStateManager, null, (x, y) => x.OnPlayerDeath(playerDefinition));
            Destroy(gameObject);
        }

        #region Root

        public void Root(int rootHardness)
        {
            StartCoroutine(RootCoroutine(rootHardness));
        }

        private IEnumerator RootCoroutine(int rootHardness)
        {
            m_animator.SetTrigger("root");
            m_animator.SetFloat("xSpeed", 0.0f);

            m_stats.root = true;
            m_stats.rootHardness = rootHardness;

            while(m_stats.root)
            {
                yield return null;

                if (m_playerControlScheme.interact.Started())
                {
                    m_stats.rootHardness -= 1;
                }

                m_stats.root = m_stats.rootHardness > 0;
            }
        }

        #endregion

        #region Impulse

        public void Impulse(Vector3 direction,float force)
        {
            StartCoroutine(ImpulseCoroutine(direction,force));
        }

        IEnumerator ImpulseCoroutine(Vector3 direction,float force)
        {
            m_animator.SetTrigger("push");
            m_animator.SetFloat("pushForce", direction.x);
            m_isBeingImpulsed = true;
            bool isRight = direction.x > 0;
            Vector3 director = Quaternion.Euler(0,0,45) * Vector3.right;
            if (!isRight)
            {
                director.x = -director.x;
            }
            director *= force;
            while(Mathf.Abs(director.x) > m_minPushMagnitude)
            {
                MoveCharacterContoller(director * Time.deltaTime);
                yield return null;
                director.x = Mathf.Lerp(director.x,0,Time.deltaTime/m_pushReduction);
                director.y += m_gravity * Time.deltaTime*m_gravityPushMuliplier;
            }
            
            m_isBeingImpulsed = false;
        }

        #endregion

        public void Debuff(StatModifier statmodifier)
        {
            m_stats.AddModifier(statmodifier);
        }

        #region Burned

        public void Burned(int burnedTime)
        {
            if (!m_stats.burned)
            {
                StartCoroutine(BurnedCoroutine(burnedTime));
            }
        }

        private IEnumerator BurnedCoroutine(int burnedTime)
        {
            m_stats.burned = true;
            yield return new WaitForSeconds(burnedTime);
            m_stats.burned = false;
        }

        #endregion

        #region WindPush

        public void ActivateWindPush()
        {
            m_stats.windPush = true;
        }

        public void DeactivateWindPush()
        {
            m_stats.windPush = false;
        }

        #endregion
    }
}
