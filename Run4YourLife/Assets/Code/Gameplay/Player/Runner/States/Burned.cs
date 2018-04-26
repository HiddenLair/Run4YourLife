using UnityEngine;

namespace Run4YourLife.Player
{
    public class Burned : RunnerState, IRunnerInput
    {
        private const float SPEED_BUFF_PERCENT = 1.0f / 3.0f;
        private float END_TIME = 5.0f;

        #region Variables

        private StatModifier modifier;
        private float timer = 0.0f;

        #endregion

        private float lastInputSign = 1.0f;

        public Burned() : base(State.Burned)
        {
        }

        int IRunnerInput.GetPriority()
        {
            return 0;
        }

        public void ModifyHorizontalInput(ref float input)
        {
            float inputSign = lastInputSign;

            if (input > 0.0f)
            {
                inputSign = 1.0f;
            }
            else if (input < 0.0f)
            {
                inputSign = -1.0f;
            }

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
            timer += Time.deltaTime;
            if (timer >= END_TIME)
            {
                Destroy(this);
            }
        }

        public void Refresh()
        {
            timer = 0.0f;
            GetComponent<Stats>().RemoveStatModifier(modifier);
            GetComponent<Stats>().AddModifier(modifier);
        }

        protected override void Apply()
        {
            modifier = new SpeedModifier(ModifierType.PERCENT, true, SPEED_BUFF_PERCENT, END_TIME);
            GetComponent<Stats>().AddModifier(modifier);
        }

        protected override void Unapply()
        {
            GetComponent<Stats>().RemoveStatModifier(modifier);
        }

        public void SetBurningTime(int burningTime)
        {
            if (burningTime > 0)
            {
                END_TIME = burningTime;
            }
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}