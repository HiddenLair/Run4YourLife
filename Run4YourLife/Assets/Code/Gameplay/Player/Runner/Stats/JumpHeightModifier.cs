namespace Run4YourLife.Player
{
    public class JumpHeightModifier : StatModifier
    {
        public JumpHeightModifier(ModifierType modifierType, bool buff, float amount, float endTime) :
            base(StatType.JUMP_HEIGHT, modifierType, buff, amount, endTime)
        { }

        public override int GetPriority()
        {
            return 2;
        }
    }
}