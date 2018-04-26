namespace Run4YourLife.Player
{
    public class BounceHeightModifier : StatModifier
    {
        public BounceHeightModifier(ModifierType modifierType, bool buff, float amount, float endTime) :
            base(StatType.BOUNCE_HEIGHT, modifierType, buff, amount, endTime)
        { }

        public override int GetPriority()
        {
            return 1;
        }
    }
}