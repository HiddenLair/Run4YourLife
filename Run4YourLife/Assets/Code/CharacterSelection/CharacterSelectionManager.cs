using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

namespace Run4YourLife.CharacterSelection
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        PlayerManager playerManager;

        void Awake()
        {
            playerManager = Object.FindObjectOfType<PlayerManager>();
            if (playerManager == null)
                Debug.LogWarning("Player Manager is NULL but should not be null");

            ControllerDetector controllerDetector = GetComponent<ControllerDetector>();
            if (controllerDetector == null)
                Debug.LogWarning("ControllerDetector is NULL but should not be null");
            controllerDetector.OnControllerDetected += OnControllerDetected;
        }

        void OnControllerDetected(Controller controller)
        {
            if (!IsAssignedToAPlayer(controller))
            {
                PlayerDefinition player = new PlayerDefinition
                {
                    Controller = controller
                };

                if (playerManager.GetPlayers().Count == 0)
                {
                    player.IsBoss = true;
                }

                playerManager.AddPlayer(player);
            }
        }

        private bool IsAssignedToAPlayer(Controller controller)
        {
            foreach(PlayerDefinition p in playerManager.GetPlayers())
            {
                if (p.Controller.Equals(controller))
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            foreach (PlayerDefinition player in playerManager.GetPlayers())
            {
                //UpdatePlayer(player);
            }
        }

        private void UpdatePlayer(PlayerDefinition player)
        {
            Controller controller = player.Controller;

            if (controller.GetButton(Controller.Button.X))
            {
                playerManager.SetPlayerAsBoss(player);
            }
            else if (controller.GetButton(Controller.Button.B))
            {
                playerManager.RemovePlayer(player);
            }
            else if (controller.GetButton(Controller.Button.Y))
            {
                BackToMainMenu();
            }
            else if (controller.GetButton(Controller.Button.R))
            {
                ChangePlayerCharacter(AdvanceType.Next);
            }
            else if(controller.GetButton(Controller.Button.L))
            {
                ChangePlayerCharacter(AdvanceType.Previous);
            }
        }

        enum AdvanceType
        {
            Next,
            Previous
        }

        private void ChangePlayerCharacter(AdvanceType advanceType)
        {
            throw new System.NotImplementedException("Still not implemented");
        }

        private void BackToMainMenu()
        {
            throw new System.NotImplementedException("Still not implemented");
        }
    }
}