using UnityEngine.Events;

namespace Run4YourLife.Input
{
    [System.Serializable]
    public class InputDeviceEvent : UnityEvent<InputDevice>
    {
    }

    public class InputDevice
    {
        uint id;

        public InputDevice(uint id)
        {
            this.id = id;
        }

        public string InputString(string input)
        {
            return "joy" + id + input;
        }
    }
}