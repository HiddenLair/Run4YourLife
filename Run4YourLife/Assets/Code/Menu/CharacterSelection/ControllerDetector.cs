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
            foreach (InputDevice inputDevice in inputDeviceManager.InputDevices()) 
            {
                if (new InputSource(Button.A, inputDevice).ButtonDown() && !IsAssignedToAPlayer(inputDevice))
                {
                    OncontrollerDetected.Invoke(inputDevice);
                }
            }
        }

        private bool IsAssignedToAPlayer(InputDevice controller)
        {
            foreach (PlayerDefinition p in playerManager.GetPlayers())
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