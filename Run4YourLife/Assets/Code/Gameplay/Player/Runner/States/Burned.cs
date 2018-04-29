using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerAttributeController))]
    public class Burned : RunnerState, IRunnerInput
    {
        private const float SPEED_BUFF_PERCENT = 1.0f / 3.0f;


        private AttributeModifier m_modifier;
        private RunnerAttributeController m_stats;

        private float lastInputSign = 1.0f;
        private float m_destructionTime;
        private float m_burnDuration;

        public override Type StateType { get { return Type.Burned; } }


        protected override void Awake()
        {
            base.Awake();
            m_stats = GetComponent<RunnerAttributeController>();
        }

        int IRunnerInput.GetPriority()
        {
            return 0;
        }

        public void ModifyHorizontalInput(ref float input)
        {
            float inputSign = Mathf.Sign(input);

            if (inputSign != lastInputSign)
            {
                input = inputSign;
                lastInputSign = inputSign;
            }
            else
            {
                input = lastInputSign;
            }
        }

        private void Update()
        {
            if (Time.time > m_destructionTime)
                Destroy(this);
        }

        public void Refresh()
        {
            m_destructionTime = Time.time + m_burnDuration;

            //TODO: Why do we remove it and re-add it?
            m_stats.RemoveAttributeModifier(m_modifier);
            m_stats.AddModifier(m_modifier);
        }

        protected override void Apply()
        {
            m_modifier = new SpeedModifier(AttributeModifierType.PERCENT, true, SPEED_BUFF_PERCENT, m_burnDuration);
            m_stats.AddModifier(m_modifier);

            m_destructionTime = Time.time + m_burnDuration;
        }

        protected override void Unapply()
        {
            m_stats.RemoveAttributeModifier(m_modifier);
        }

        public void SetBurningTime(int burningTime)
        {
            if (burningTime > 0)
            {
                m_burnDuration = burningTime;
            }
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}