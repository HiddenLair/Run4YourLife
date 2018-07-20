using UnityEngine.EventSystems;

using Run4YourLife.InputManagement;

namespace Run4YourLife.Player
{
    public interface IPlayerHandleEvent : IEventSystemHandler
    {
        void OnPlayerHandleChanged(PlayerHandle playerHandle);
    }

    public enum CharacterType
    {
        ACorn,
        Skull,
        Snake,
        Plain,
        NoColor
    }

    public class PlayerHandle
    {
        public uint ID { get; set; }
        public InputDevice InputDevice { get; set; }
        public bool IsBoss { get; set; }
        public CharacterType CharacterType { get; set; }

        public static readonly PlayerHandle DebugDefaultPlayerHandle = new PlayerHandle() {
            CharacterType = CharacterType.ACorn,
            ID = 1,
            IsBoss = false,
            InputDevice = Run4YourLife.InputManagement.InputDeviceManager.Instance.DefaultInputDevice
        };
    }
}