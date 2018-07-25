using UnityEngine;
using Run4YourLife;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class FallingPlatformController : MonoBehaviour, IBossDestructibleNotified
    {

        [SerializeField]
        private GameObject m_FallingCollider;

        [SerializeField]
        public float m_delay;

        private Rigidbody m_rigidbody;

        private Animator m_animator;

        private bool activatedFlag = false;
        private Vector3 m_startingPosition;

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
            m_rigidbody = GetComponent<Rigidbody>();
            Debug.Assert(m_rigidbody != null);
            m_startingPosition = m_rigidbody.transform.localPosition;
        }

        public void OnRunnerWalkedOnTop()
        {
            if (!activatedFlag)
            {
                StartCoroutine(YieldHelper.WaitForSeconds(Fall, m_delay));
            }
        }

        private void Fall()
        {
            m_animator.enabled = false;
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
            m_FallingCollider.SetActive(true);
            activatedFlag = true;
        }

        private void ResetState()
        {
            m_rigidbody.transform.localPosition = m_startingPosition;

            m_rigidbody.useGravity = false;
            m_rigidbody.isKinematic = true;
            m_FallingCollider.SetActive(false);
            activatedFlag = false;
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
