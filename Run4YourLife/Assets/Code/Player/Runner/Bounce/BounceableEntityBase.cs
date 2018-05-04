using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player {

    [RequireComponent(typeof(Collider))]
    public abstract class BounceableEntityBase : MonoBehaviour {

        [SerializeField]
        private bool falling = false;

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
                if (!falling || runnerCharacterController.Velocity.y < 0)
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
