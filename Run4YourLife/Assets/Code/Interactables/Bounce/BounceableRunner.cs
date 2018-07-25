using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables
{
    public class BounceableRunner : BounceableEntityBase
    {

        private RunnerController m_runnerCharacterController;
        private RunnerAttributeController m_runnerAttributeController;
        private BumpController m_bumpController;

        private Collider m_collider;

        public override Vector3 BounceForce
        {
            get
            {
                return new Vector3()
                {
                    y = m_runnerAttributeController.GetAttribute(RunnerAttribute.BounceHeight)
                };
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_collider = GetComponent<Collider>();
            Debug.Assert(m_collider != null);

            m_bumpController = GetComponentInParent<BumpController>();
            Debug.Assert(m_bumpController != null);

            m_runnerAttributeController = GetComponentInParent<RunnerAttributeController>();
            Debug.Assert(m_runnerAttributeController != null);

            m_runnerCharacterController = GetComponentInParent<RunnerController>();
            Debug.Assert(m_runnerCharacterController != null);

            // We ignore collisions with the runner because otherwise we would collide with it all the time
            Physics.IgnoreCollision(m_collider, GetComponentInParent<Collider>());
        }

        public override bool ShouldBounceByContact(RunnerController runnerCharacterController)
        {
            return runnerCharacterController.Velocity.y < 0;
        }

        public override void BouncedOn()
        {
            base.BouncedOn();
            m_bumpController.Bump();
        }

        public override Vector3 GetStartingBouncePosition(RunnerController runnerCharacterController)
        {
            Vector3 startingBouncePosition = runnerCharacterController.transform.position;
            startingBouncePosition.y = m_collider.bounds.max.y;
            return startingBouncePosition;
        }
    }
}