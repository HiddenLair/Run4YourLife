using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Interactables;

namespace Run4YourLife.Player.Runner.Ghost
{
    [RequireComponent(typeof(Collider))]
    public class BounceableRunnerGhost : BounceableEntityBase
    {
        [SerializeField]
        private float m_bounceForce;

        [SerializeField]
        private Collider m_collider;
        
        [SerializeField]
        private RunnerGhostController m_runnerGhostController;

        protected override void Awake()
        {
            base.Awake();
            m_collider = GetComponent<Collider>();
            m_runnerGhostController = GetComponentInParent<RunnerGhostController>();
            Debug.Assert(m_runnerGhostController != null);
        }

        public override Vector3 BounceForce
        {
            get
            {
                return new Vector3()
                {
                    y = m_bounceForce
                };
            }
        }

        public override void BouncedOn()
        {
            base.BouncedOn();
            m_runnerGhostController.ReviveRunner();
        }

        public override Vector3 GetStartingBouncePosition(RunnerController runnerCharacterController)
        {
            Vector3 startingBouncePosition = runnerCharacterController.transform.position;
            startingBouncePosition.y = m_collider.bounds.max.y;
            return startingBouncePosition;
        }

        public override bool ShouldBounceByContact(RunnerController runnerCharacterController)
        {
            return runnerCharacterController.Velocity.y < 0;
        }
    }
}
