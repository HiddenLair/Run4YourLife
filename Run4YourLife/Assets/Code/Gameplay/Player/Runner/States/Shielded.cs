using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BuffManager))]
    public class Shielded : RunnerState
    {
        private const float END_TIME = 5.0f;

        private BuffManager m_buffManager;

        public override Type StateType { get { return Type.Shielded; } }

        protected override void Awake()
        {
            base.Awake();
            m_buffManager = GetComponent<BuffManager>();
        }

        protected override void Apply()
        {
            m_buffManager.SubscribeBuff(this);
            m_buffManager.ActivateShield();
            Destroy(this, END_TIME);
        }

        protected override void Unapply()
        {
            m_buffManager.UnsubscribeBuff(this);
            m_buffManager.DeactivateShield();
        }
    }
}


