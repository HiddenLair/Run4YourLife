using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private Transform spawnLocation;

        private void Awake()
        {
            PlayerManager playerManger = GetOrCreateDefaultPlayerManager();
            Debug.Assert(playerManger != null);

            InstantiatePlayers(playerManger.GetPlayers());
        }

        private PlayerManager GetOrCreateDefaultPlayerManager()
        {
            //TODO if no playermanager is found, create default player manager
            //useful for debug opening the scene
            return FindObjectOfType<PlayerManager>();
        }

        private void InstantiatePlayers(List<PlayerDefinition> playerDefinitions)
        {
            foreach(PlayerDefinition playerDefinition in playerDefinitions)
            {
                InstantiatePlayer(playerDefinition);
            }
        }

        private void InstantiatePlayer(PlayerDefinition playerDefinition)
        {
            GameObject player = InstantiatePlayerByType(playerDefinition.CharacterType, playerDefinition.IsBoss);
            ExecuteEvents.Execute<IPlayerDefinitionEvents>(player, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
        }

        private GameObject InstantiatePlayerByType(CharacterType characterType, bool isBoss)
        {
            return Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        }
    }
}
