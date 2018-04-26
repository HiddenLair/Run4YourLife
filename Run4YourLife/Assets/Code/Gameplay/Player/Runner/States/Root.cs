using System;

namespace Run4YourLife.Player
{
    public class Root : RunnerState, IInteractInput, IJumpInput, IVerticalInput
    {
        #region Variables

        private int remainingHits = 4;

        StatModifier modifierSpeed;

        #endregion

        public override Type StateType { get { return Type.Root; } }

        int IInteractInput.GetPriority()
        {
            return 1;
        }

        int IJumpInput.GetPriority()
        {
            return 1;
        }

        int IVerticalInput.GetPriority()
        {
            return 1;
        }

        public void ModifyInteractInput(ref bool input)
        {
            if (input)
            {
                if (--remainingHits == 0)
                {
                    Destroy(this);
                }
            }

            input = false;
        }

        public void ModifyVerticalInput(ref float input)
        {
            input = 0.0f;
        }

        public void ModifyJumpInput(ref bool input)
        {
            input = false;
        }

        protected override void Apply()
        {
            modifierSpeed = new SpeedModifier(ModifierType.SETTER, true, 0, -1);
            GetComponent<Stats>().AddModifier(modifierSpeed);
        }

        protected override void Unapply()
        {
            GetComponent<Stats>().RemoveStatModifier(modifierSpeed);
        }

        public void SetHardness(int rootHardness)
        {
            if (rootHardness > 0)
            {
                remainingHits = rootHardness;
            }
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}