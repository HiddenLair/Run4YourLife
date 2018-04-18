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

        public bool HasInput()
        {
            InputSource inputSource = new InputSource(this);

            foreach (Trigger trigger in Trigger.TRIGGERS)
            {
                inputSource.input = trigger.ID;
                if (inputSource.Value() != 0)
                {
                    return true;
                }
            }

            foreach (Axis axis in Axis.AXES)
            {
                inputSource.input = axis.ID;
                if (inputSource.Value() != 0)
                {
                    return true;
                }
            }

            foreach (Button button in Button.BUTTONS)
            {
                inputSource.input = button.ID;
                if (inputSource.ButtonDown() || inputSource.Button() || inputSource.ButtonUp())
                {
                    return true;
                }
            }

            return false;
        }
    }
}