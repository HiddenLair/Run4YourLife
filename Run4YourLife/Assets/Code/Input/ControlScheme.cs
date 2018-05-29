using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.InputManagement
{
    public abstract class ControlScheme : MonoBehaviour, IPlayerHandleEvent
    {
        private List<InputAction> m_inputActions = new List<InputAction>();
        public List<InputAction> InputActions { get { return m_inputActions; } }

        private bool m_active;
        public bool Active
        {
            get
            {
                return m_active;
            }

            set
            {
                if (value != m_active)
                {
                    m_active = value;
                    foreach (InputAction inputAction in m_inputActions)
                    {
                        inputAction.Enabled = value;
                    }
                }
            }
        }

        private InputDevice m_inputDevice;
        public InputDevice InputDevice {
            get
            {
                return m_inputDevice;
            }

            set
            {
                UpdateInputDevice(value);
            }
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
            InputDevice = playerHandle != null ? playerHandle.InputDevice : null;
        }

        protected void UpdateInputDevice(InputDevice inputDevice)
        {
            m_inputDevice = inputDevice;
            foreach (InputAction action in m_inputActions)
            {
                action.InputSource.InputDevice = inputDevice;
            }
        }

        public InputAction GetByName(string name)
        {
            return m_inputActions.Find((x) => x.Name.Equals(name));
        }
    }
}