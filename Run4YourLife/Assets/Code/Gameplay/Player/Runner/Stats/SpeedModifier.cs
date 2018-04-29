namespace Run4YourLife.Player
{
    public class SpeedModifier : AttributeModifier
    {
        protected override AttributeType AttributeType { get { return AttributeType.SPEED; } }

        public SpeedModifier(AttributeModifierType modifierType, bool buff, float amount, float endTime) :
            base(modifierType, buff, amount, endTime)
        { }


        public override int GetPriority()
        {
            return 0;
        }
    }
}