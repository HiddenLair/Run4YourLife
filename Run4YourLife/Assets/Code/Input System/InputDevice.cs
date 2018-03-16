using UnityEngine.Events;

namespace Run4YourLife.Input
{
    [System.Serializable]
    public class InputDeviceEvent : UnityEvent<InputDevice>
    {
    }

    public class InputDevice
    {
        private uint id;
        
        public uint ID
        {
            get
            {
                return id;
            }

            private set
            {
                id = value;
            }
        }

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