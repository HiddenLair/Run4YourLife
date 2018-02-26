using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public abstract class ControlScheme : MonoBehaviour
    {
        public InputDevice inputDevice { get; set; }

        protected List<Action> actions = new List<Action>();

        public void Activate()
        {
            foreach (Action action in actions)
            {
                action.enabled = true;
            }
        }

        public void Deactivate()
        {
            foreach (Action action in actions)
            {
                action.enabled = false;
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