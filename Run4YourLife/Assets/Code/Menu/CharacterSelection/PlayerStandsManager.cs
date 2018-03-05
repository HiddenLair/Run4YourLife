using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            Debug.Assert(playerManager != null);
            playerManager.OnPlayerChanged.AddListener(OnPlayerChanged);
        }

        private void Start()
        {
            OnPlayerChanged();
        }

        void OnPlayerChanged()
        {
            DestroyCurrentStants();
            SpawnNewStands();
        }

        private void DestroyCurrentStants()
        {
            foreach (PlayerStandController stand in standControllers)
            {
                ExecuteEvents.Execute<IPlayerDefinitionEvents>(stand.gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(null));
            }
        }

        private void SpawnNewStands()
        {
            List<PlayerDefinition> players = playerManager.GetPlayers();
            Debug.Assert(players.Count <= standControllers.Length);

            for (int i = 0; i < players.Count; i++)
            {
                ExecuteEvents.Execute<IPlayerDefinitionEvents>(standControllers[i].gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(players[i]));
            }
        }
    }
}