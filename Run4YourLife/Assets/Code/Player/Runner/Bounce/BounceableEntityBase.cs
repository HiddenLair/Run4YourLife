using System.Collections;
using System.Collections.Generic;
using Run4YourLife.InputManagement;
using UnityEngine;

namespace Run4YourLife.Player {

    [RequireComponent(typeof(Collider))]
    public abstract class BounceableEntityBase : MonoBehaviour {

        [SerializeField]
        [Tooltip("Allways bounce when colliding with it")]
        private bool m_allwaysBounce;

        [SerializeField]
        [Tooltip("When colliding with it check wether the runner has the jump button pressed or not. If set to positive will only jump when pressed")]
        private bool m_shouldBeJumping;

        protected abstract Vector3 BounceForce { get; }

        private Collider m_collider;

        protected virtual void Awake()
        {
            m_collider = GetComponent<Collider>();
            Debug.Assert(m_collider != null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                RunnerCharacterController runnerCharacterController = other.GetComponent<RunnerCharacterController>();
                if (m_allwaysBounce || ((!m_shouldBeJumping || m_shouldBeJumping && runnerCharacterController.GetComponent<RunnerControlScheme>().Jump.Persists()) && runnerCharacterController.Velocity.y < 0))
                {
                    FixRunnerPositionOnTopOfEntity(runnerCharacterController);
                    runnerCharacterController.Bounce(BounceForce);
                    BouncedOn();
                }
            }
        }

        private void FixRunnerPositionOnTopOfEntity(RunnerCharacterController runnerCharacterController)
        {
            Vector3 runnerPosition = runnerCharacterController.transform.position;
            runnerPosition.y = m_collider.bounds.max.y;
            runnerCharacterController.transform.position = runnerPosition;
        }

        protected abstract void BouncedOn();
    }
}
