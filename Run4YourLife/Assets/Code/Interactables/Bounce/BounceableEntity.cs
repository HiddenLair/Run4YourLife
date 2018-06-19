using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.InputManagement;

namespace Run4YourLife.Interactables
{
    public class BounceableEntity : BounceableEntityBase {
        private enum BounceTrigger {
            Allways,
            NegativeVerticalVelocity
        }

        [SerializeField]
        private BounceTrigger m_bounceTrigger;

        [SerializeField]
        [Range(0,360)]
        private float m_angle;

        [SerializeField]
        private float m_bounceForce;

        private Animator m_animator;
        private Collider m_collider;

        protected override void Awake()
        {
            base.Awake();
            m_animator = GetComponentInChildren<Animator>();
            Debug.Assert(m_animator != null);

            m_collider = GetComponentInChildren<Collider>();
            Debug.Assert(m_collider != null);
        }

        public override Vector3 BounceForce
        {
            get
            {
                return m_bounceForce * (Quaternion.Euler(0, 0, m_angle) * Vector3.right);
            }
        }

        public override void BouncedOn()
        {
            base.BouncedOn();
            m_animator.SetTrigger("bump");
        }

        public override bool ShouldBounceByContact(RunnerController runnerCharacterController)
        {
            return m_bounceTrigger == BounceTrigger.Allways || (m_bounceTrigger == BounceTrigger.NegativeVerticalVelocity && runnerCharacterController.Velocity.y < 0);
        }

        public override Vector3 GetStartingBouncePosition(RunnerController runnerCharacterController)
        {
            Vector3 startingBouncePosition = runnerCharacterController.transform.position;
            startingBouncePosition.y = m_collider.bounds.max.y;
            return startingBouncePosition;
        }
    }
}