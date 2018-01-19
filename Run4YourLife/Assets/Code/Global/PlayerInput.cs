using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.General;

namespace Run4YourLife.GameInput
{
    public class PlayerInput : MonoBehaviour
    {
        private Controller[] controllers;

        private List<Controller> unregisteredControllers;
        public List<Controller> UnregisteredControllers { get { return unregisteredControllers; } }

        private Dictionary<Player, Controller> playerToController;

        void Awake()
        {
            controllers = new Controller[4] {
                new Controller(1),
                new Controller(2),
                new Controller(3),
                new Controller(4)
            };

            unregisteredControllers.AddRange(controllers);
        }

        public void RegisterControllerToPlayer(Player player, Controller controller)
        {
            Controller currentPlayerController;
            playerToController.TryGetValue(player, out currentPlayerController);
            if (currentPlayerController != null)
            {
                Debug.LogError("Trying to register controller to player but player already has a controller");
            }
            else
            {
                playerToController[player] = controller;
                unregisteredControllers.Remove(currentPlayerController);
            }
        }

        public void UnregisterPlayerController(Player player)
        {
            Controller currentPlayerController;
            playerToController.TryGetValue(player, out currentPlayerController);
            if (currentPlayerController == null)
                Debug.LogError("Trying to unregisetr player controller but player does not have a controller");
            else
            {
                playerToController.Remove(player);
                unregisteredControllers.Add(currentPlayerController);
            }
        }

        public void UnregisterAllPlayers()
        {
            playerToController.Clear();
            unregisteredControllers.Clear();
            unregisteredControllers.AddRange(controllers);
        }

        public Controller GetControllerForPlayer(Player player)
        {
            Controller currentPlayerController;
            playerToController.TryGetValue(player, out currentPlayerController);
            if (currentPlayerController == null)
                Debug.LogError("Trying to get player controller but player did not have a controller");

            return currentPlayerController;
        }
    }

}