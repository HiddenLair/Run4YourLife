using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.Input;

namespace Run4YourLife.CharacterSelection
{
    public class ControllerDetector : MonoBehaviour {
        public InputDeviceEvent OncontrollerDetected;

        private InputDeviceManager inputDeviceManager;
        private PlayerManager playerManager;

        void Awake()
        {
            inputDeviceManager = Component.FindObjectOfType<InputDeviceManager>();
            Debug.Assert(inputDeviceManager != null);

            playerManager = Component.FindObjectOfType<PlayerManager>();
            Debug.Assert(playerManager != null);
        }

        void Update()
        {
            InputSource joinGameInput = new InputSource(Button.A);
            foreach (InputDevice inputDevice in inputDeviceManager.InputDevices) 
            {
                joinGameInput.inputDevice = inputDevice;
                if (joinGameInput.ButtonDown() && !IsAssignedToAPlayer(inputDevice))
                {
                    OncontrollerDetected.Invoke(inputDevice);
                }
            }
        }

        private bool IsAssignedToAPlayer(InputDevice controller)
        {
            foreach (PlayerHandle p in playerManager.GetPlayers())
            {
                if (p.inputDevice.Equals(controller))
                {
                    return true;
                }
            }
            return false;
        }
    }
}