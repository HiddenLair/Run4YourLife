using UnityEngine.EventSystems;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    public interface IPlayerHandleEvent : IEventSystemHandler
    {
        void OnPlayerDefinitionChanged(PlayerHandle playerDefinition);
    }

    public enum CharacterType
    {
        Purple,
        Green,
        Orange
    }

    public class PlayerHandle
    {
        public uint ID;
        public InputDevice inputDevice;
        public bool IsBoss { get; set; }
        public CharacterType CharacterType { get; set; }

        public static readonly PlayerHandle DebugDefaultPlayerHandle = new PlayerHandle() { 
                CharacterType = CharacterType.Purple,
                ID = 1,
                IsBoss = false,
                inputDevice = new InputDevice(2)
            };
    }
}