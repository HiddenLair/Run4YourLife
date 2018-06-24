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
        White,
        Purple,
        Red,
        Green
    }

    public class PlayerHandle
    {
        public uint ID { get; set; }
        public InputDevice InputDevice { get; set; }
        public bool IsBoss { get; set; }
        public CharacterType CharacterType { get; set; }

        public static readonly PlayerHandle DebugDefaultPlayerHandle = new PlayerHandle() {
            CharacterType = CharacterType.Purple,
            ID = 1,
            IsBoss = false,
            InputDevice = Run4YourLife.InputManagement.InputDeviceManager.Instance.DefaultInputDevice
        };
    }
}