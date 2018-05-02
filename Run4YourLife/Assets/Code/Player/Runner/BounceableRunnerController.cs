using UnityEngine;

namespace Run4YourLife.Player
{
    public class BounceableRunnerController : BounceableEntityBase {

        private RunnerCharacterController m_runnerCharacterController;
        private RunnerAttributeController m_runnerAttributeController;

        protected override Vector3 BounceForce
        {
            get
            {
                float bounceHeight = m_runnerAttributeController.GetAttribute(RunnerAttribute.BounceHeight);
                return Vector3.up * bounceHeight;
            }
        }

        private void Awake()
        {
            m_runnerAttributeController = GetComponentInParent<RunnerAttributeController>();
            UnityEngine.Debug.Assert(m_runnerAttributeController != null);

            m_runnerCharacterController = transform.parent.GetComponent<RunnerCharacterController>();
            UnityEngine.Debug.Assert(m_runnerCharacterController != null, "Objects needs a parent that has a player character controller");

            // We ignore collisions with the runner because otherwise we would collide with it all the time
            Physics.IgnoreCollision(GetComponent<Collider>(), m_runnerCharacterController.GetComponent<Collider>());
        }

        protected override void BouncedOn()
        {
            m_runnerCharacterController.BouncedOn();
        }
    }
}
