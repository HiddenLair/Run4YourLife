using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public abstract class ControlScheme : MonoBehaviour, IPlayerHandleEvent
    {
        private List<Action> m_actions = new List<Action>();
        public List<Action> Actions { get { return m_actions; } }

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
                    foreach (Action action in m_actions)
                    {
                        action.Enabled = value;
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
            InputDevice = playerHandle != null ? playerHandle.inputDevice : null;
        }

        protected void UpdateInputDevice(InputDevice inputDevice)
        {
            m_inputDevice = inputDevice;
            foreach (Action action in m_actions)
            {
                action.InputSource.InputDevice = inputDevice;
            }
        }

        public Action GetByName(string name)
        {
            return m_actions.Find((x) => x.Name.Equals(name));
        }
    }
}