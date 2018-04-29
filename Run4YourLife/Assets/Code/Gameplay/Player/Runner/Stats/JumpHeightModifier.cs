namespace Run4YourLife.Player
{
    public class JumpHeightModifier : AttributeModifier
    {
        protected override AttributeType AttributeType { get { return AttributeType.JUMP_HEIGHT; } }

        public JumpHeightModifier(AttributeModifierType modifierType, bool buff, float amount, float endTime) :
            base(modifierType, buff, amount, endTime)
        { }

        public override int GetPriority()
        {
            return 2;
        }
    }
}