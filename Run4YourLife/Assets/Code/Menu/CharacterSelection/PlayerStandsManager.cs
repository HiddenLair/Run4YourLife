using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using System;

namespace Run4YourLife.CharacterSelection
{
    public class PlayerStandsManager : MonoBehaviour
    {
        PlayerManager playerManager;

        [SerializeField]
        private PlayerStandController[] standControllers;

        private void Awake()
        {
            playerManager = Component.FindObjectOfType<PlayerManager>();
            playerManager.OnPlayerChange += OnPlayerChange;
        }

        void OnPlayerChange()
        {
            DestroyCurrentStants();
            SpawnNewStands();
        }

        private void DestroyCurrentStants()
        {
            foreach (PlayerStandController stand in standControllers)
            {
                stand.ClearPlayerDefinition();
            }
        }

        private void SpawnNewStands()
        {
            List<PlayerDefinition> players = playerManager.GetPlayers();
            Debug.Assert(players.Count <= standControllers.Length);

            for (int i = 0; i < players.Count; i++)
            {
                standControllers[i].SetPlayerDefinition(players[i]);
            }
        }
    }
}