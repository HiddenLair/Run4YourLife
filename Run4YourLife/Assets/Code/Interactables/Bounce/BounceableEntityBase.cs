using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables
{
    public abstract class BounceableEntityBase : MonoBehaviour, IBounceable
    {
        public abstract Vector3 BounceForce { get; }

        private FXReceiver m_bounceParticle;

        protected virtual void Awake()
        {
            m_bounceParticle = GetComponentInChildren<FXReceiver>();
            Debug.Assert(m_bounceParticle != null);
        }

        public abstract Vector3 GetStartingBouncePosition(RunnerController runnerCharacterController);
        public abstract bool ShouldBounceByContact(RunnerController runnerCharacterController);
        public virtual void BouncedOn()
        {
            m_bounceParticle.PlayFx(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + BounceForce);
        }
    }
}
