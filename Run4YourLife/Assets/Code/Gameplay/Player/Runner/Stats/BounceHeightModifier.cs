namespace Run4YourLife.Player
{
    public class BounceHeightModifier : AttributeModifier
    {
        protected override AttributeType AttributeType { get { return AttributeType.BOUNCE_HEIGHT; } }

        public BounceHeightModifier(AttributeModifierType modifierType, bool buff, float amount, float endTime) :
            base(modifierType, buff, amount, endTime)
        { }

        public override int GetPriority()
        {
            return 1;
        }
    }
}