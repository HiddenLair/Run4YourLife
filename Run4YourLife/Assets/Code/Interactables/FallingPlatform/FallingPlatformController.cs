using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class FallingPlatformController : MonoBehaviour, IBossDestructibleNotified
    {
        [SerializeField]
        private float m_startingSpeed;

        [SerializeField]
        private float m_gravity;

        [SerializeField]
        public float m_delay;

        [SerializeField]
        private TrembleConfig m_collideGroundTrembleConfig;

        [SerializeField]
        private AudioClip m_collideGroundSFX;

        private Animator m_animator;

        private float m_currentSpeed;
        private bool m_activated;
        private Vector3 m_startingPosition;

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
            m_startingPosition = transform.localPosition;
            m_currentSpeed = m_startingSpeed;
        }

        public void OnRunnerWalkedOnTop()
        {
            if (!m_activated)
            {
                m_activated = true;
                StartCoroutine(YieldHelper.WaitForSeconds(Fall, m_delay));
            }
        }

        private void Fall()
        {
            StartCoroutine(FallBehaviour());
        }

        private IEnumerator FallBehaviour()
        {
            m_animator.enabled = false;
            yield return new WaitForSeconds(m_delay);

            Vector3 endPosition = FindEndPosition();
            while (transform.position != endPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, m_currentSpeed * Time.deltaTime);
                m_currentSpeed += m_gravity * Time.deltaTime;
                yield return null;
            }

            //Collide ground
            TrembleManager.Instance.Tremble(m_collideGroundTrembleConfig);
            AudioManager.Instance.PlaySFX(m_collideGroundSFX);
        }

        private Vector3 FindEndPosition()
        {
            RaycastHit info;
            bool found = Physics.Raycast(transform.position, Vector3.down, out info, Mathf.Infinity, Layers.Stage);
            Debug.Assert(found);
            return info.point;
        }

        private void ResetState()
        {
            StopAllCoroutines();

            m_animator.enabled = true;
            transform.localPosition = m_startingPosition;
            m_activated = false;
            m_currentSpeed = m_startingSpeed;
        }

        void IBossDestructibleNotified.OnDestroyed()
        {
        }

        void IBossDestructibleNotified.OnRegenerated()
        {
            ResetState();
        }
    }
}
