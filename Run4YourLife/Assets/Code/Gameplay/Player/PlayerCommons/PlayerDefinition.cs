using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    public enum CharacterType
    {
        Purple,
        Green,
        Orange
    }

    public class PlayerDefinition
    {
        public uint ID;
        public InputDevice inputDevice;
        public bool IsBoss { get; set; }
        public CharacterType CharacterType { get; set; }
    }
}