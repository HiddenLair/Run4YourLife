using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

namespace Run4YourLife.CharacterSelection
{
    public class ControllerDetector : MonoBehaviour {

        public delegate void dgController(Controller controller);
        public event dgController OnControllerDetected;

        private ControllerManager playerInput;
        private PlayerManager playerManager;

        void Awake()
        {
            playerInput = Component.FindObjectOfType<ControllerManager>();
            Debug.Assert(playerInput != null);

            playerManager = Component.FindObjectOfType<PlayerManager>();
            Debug.Assert(playerManager != null);
        }

        void Update()
        {            
            foreach (Controller controller in playerInput.GetControllers()) 
            {
                if (controller.GetButtonDown(Button.A) && !IsAssignedToAPlayer(controller))
                {
                    Debug.Assert(controller != null);

                    if (OnControllerDetected != null)
                        OnControllerDetected(controller);
                }
            }
        }

        private bool IsAssignedToAPlayer(Controller controller)
        {
            foreach (PlayerDefinition p in playerManager.GetPlayers())
            {
                if (p.Controller.Equals(controller))
                {
                    return true;
                }
            }
            return false;
        }
    }
}