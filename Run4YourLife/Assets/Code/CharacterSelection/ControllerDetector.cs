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

        void Awake()
        {
            playerInput = GameObject.FindGameObjectWithTag("GlobalManager").GetComponent<ControllerManager>();
            if (playerInput == null)
                Debug.LogError(playerInput);
        }

        void Update()
        {
            foreach(Controller controller in playerInput.GetControllers()) 
            {
                if (controller.GetButtonDown(Controller.Button.A))
                {
                    if (OnControllerDetected != null)
                        OnControllerDetected(controller);
                }
            }
        }
    }
}