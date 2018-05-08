using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.Input;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class InputDeviceDetector : MonoBehaviour {
        public InputDeviceEvent OncontrollerDetected;

        void Update()
        {
            InputSource joinGameInput = new InputSource(Button.A);
            foreach (InputDevice inputDevice in InputDeviceManager.Instance.InputDevices) 
            {
                joinGameInput.InputDevice = inputDevice;
                if (joinGameInput.ButtonDown() && !IsAssignedToAPlayer(inputDevice))
                {
                    OncontrollerDetected.Invoke(inputDevice);
                }
            }
        }

        private bool IsAssignedToAPlayer(InputDevice inputDevice)
        {
            foreach (PlayerHandle playerHandle in PlayerManager.Instance.PlayerHandles)
            {
                if (playerHandle.inputDevice.Equals(inputDevice))
                {
                    return true;
                }
            }
            return false;
        }
    }
}