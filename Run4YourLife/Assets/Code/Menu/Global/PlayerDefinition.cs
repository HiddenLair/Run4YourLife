using Run4YourLife.GameInput;

namespace Run4YourLife.Player
{
    public enum CharacterType
    {
        Red,
        Green,
        Blue,
        Orange
    }

    public class PlayerDefinition
    {
        public uint ID;
        public Controller Controller { get; set; }

        public bool IsBoss { get; set; }
        public CharacterType CharacterType { get; set; }
    }
}