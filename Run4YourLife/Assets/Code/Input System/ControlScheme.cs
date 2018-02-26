using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public abstract class ControlScheme : MonoBehaviour
    {
        public InputDevice InputDevice { get; set; }

        private bool m_active;
        public bool Active
        {
            get
            {
                return m_active;
            }

            set
            {
                if(value != m_active)
                {
                    m_active = value;
                    foreach (Action action in actions)
                    {
                        action.enabled = value;
                    }
                }
            }
        }

        protected List<Action> actions = new List<Action>();

        protected void InitializeActionsWithPlayerInputDevice()
        {
            InputDevice inputDevice = GetComponent<PlayerInstance>().PlayerDefinition.inputDevice;
            foreach (Action action in actions)
            {
                action.inputSource.inputDevice = inputDevice;
            }
        }

        protected void InitializeActionsWithPlayerInputDevice()
        {
            InputDevice inputDevice = GetComponent<PlayerInstance>().PlayerDefinition.inputDevice;
            foreach (Action action in actions)
            {
                action.inputSource.inputDevice = inputDevice;
            }
        }
    }

}