using System.Text;
using UnityEngine.Events;

namespace Run4YourLife.Input
{
    [System.Serializable]
    public class InputDeviceEvent : UnityEvent<InputDevice>
    {
    }

    public class InputDevice
    {
        private StringBuilder m_stringBuilder = new StringBuilder();

        private uint m_uid;
        private string m_sid;
        public uint ID
        {
            get
            {
                return m_uid;
            }
        }

        public InputDevice(uint id)
        {
            m_uid = id;
            m_sid = id.ToString();
        }

        public string InputString(string input)
        {
            m_stringBuilder.Length = 0;
            m_stringBuilder.Append(m_sid);
            m_stringBuilder.Append(input);
            return m_stringBuilder.ToString();
        }

        public bool HasInput()
        {
            InputSource inputSource = new InputSource(this);

            foreach (Trigger trigger in Trigger.TRIGGERS)
            {
                inputSource.Input = trigger.ID;
                if (inputSource.Value() != 0)
                {
                    return true;
                }
            }

            foreach (Axis axis in Axis.AXES)
            {
                inputSource.Input = axis.ID;
                if (inputSource.Value() != 0)
                {
                    return true;
                }
            }

            foreach (Button button in Button.BUTTONS)
            {
                inputSource.Input = button.ID;
                if (inputSource.ButtonDown() || inputSource.Button() || inputSource.ButtonUp())
                {
                    return true;
                }
            }

            return false;
        }
    }
}