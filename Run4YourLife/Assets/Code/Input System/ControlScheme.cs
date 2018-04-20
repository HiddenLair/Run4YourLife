using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public abstract class ControlScheme : MonoBehaviour, IPlayerHandleEvent
    {
        protected List<Action> actions = new List<Action>();

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
                    foreach (Action action in actions)
                    {
                        action.enabled = value;
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

        public void OnPlayerDefinitionChanged(PlayerHandle playerDefinition)
        {
            InputDevice = playerDefinition != null ? playerDefinition.inputDevice : null;
        }

        protected void UpdateInputDevice(InputDevice inputDevice)
        {
            m_inputDevice = inputDevice;
            foreach (Action action in actions)
            {
                action.inputSource.inputDevice = inputDevice;
            }
        }
    }
}