namespace Run4YourLife.Player
{
    public class SpeedModifier : StatModifier
    {
        public SpeedModifier(ModifierType modifierType, bool buff, float amount, float endTime) :
            base(StatType.SPEED, modifierType, buff, amount, endTime)
        { }

        public override int GetPriority()
        {
            return 0;
        }
    }
}