using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using System;

namespace Run4YourLife.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        #region PlayerSpawning

        [SerializeField]
        private GameObject blueRunnerPrefab;

        [SerializeField]
        private GameObject redRunnerPrefab;

        [SerializeField]
        private GameObject greenRunnerPrefab;

        [SerializeField]
        private GameObject orangeRunnerPrefab;

        [SerializeField]
        private GameObject bossPrefab;

        [SerializeField]
        private Transform[] runnerSpawn;

        [SerializeField]
        private Transform spawnLocationBoss;

        #endregion

        #region Initialization

        private void Awake()
        {
            PlayerManager playerManger = GetOrCreateDefaultPlayerManager();
            Debug.Assert(playerManger != null);

            InstantiatePlayers(playerManger.GetPlayers());
        }

        private PlayerManager GetOrCreateDefaultPlayerManager()
        {
            //TODO if no playermanager is found, create default player manager
            //useful for debug opening the scene+
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            if(playerManager == null)
            {
                playerManager = gameObject.AddComponent<PlayerManager>();
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Red,
                    ID = 1,
                    inputDevice = new Input.InputDevice(1),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Orange,
                    ID = 2,
                    inputDevice = new Input.InputDevice(2),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Green,
                    ID = 3,
                    inputDevice = new Input.InputDevice(3),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Blue,
                    ID = 4,
                    inputDevice = new Input.InputDevice(4),
                    IsBoss = true
                });
            }
            return playerManager;
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
            GameObject player = InstantiatePlayerByType(playerDefinition);
            ExecuteEvents.Execute<IPlayerDefinitionEvents>(player, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
        }

        private GameObject InstantiatePlayerByType(PlayerDefinition playerDefinition)
        {
            GameObject instance;

            if(playerDefinition.IsBoss)
            {
                instance = Instantiate(bossPrefab, spawnLocationBoss.position, spawnLocationBoss.rotation);
                Camera.main.GetComponent<CameraBossFollow>().boss = instance.transform; // TODO: Temporal camera attachment
            } 
            else
            {
                GameObject runnerPrefab = null;
                switch (playerDefinition.CharacterType)
                {
                    case CharacterType.Blue:
                        runnerPrefab = bossPrefab;
                        break;
                    case CharacterType.Green:
                        runnerPrefab = greenRunnerPrefab;
                        break;
                    case CharacterType.Orange:
                        runnerPrefab = orangeRunnerPrefab;
                        break;
                    case CharacterType.Red:
                        runnerPrefab = redRunnerPrefab;
                        break;
                }
                // TODO implementation of spawn that does not require 3 transforms instead of 4
                Transform spawn = runnerSpawn[playerDefinition.ID - 1];
                instance = Instantiate(runnerPrefab, spawn.position, spawn.rotation);
            }

            return instance; 
        }

        #endregion

        #region Phase1

        public void OnEnteredPhase1()
        {

        }

        #endregion

        #region Phase2

        public GameObject phase2BossPrefab;
        public Transform phase2BossSpawnTransform;

        public GameObject phase1to2Bridge;

        public void OnEnteredPhase2()
        {
            StartCoroutine(StartPhase2());
        }

        private IEnumerator StartPhase2()
        {
            Instantiate(phase2BossPrefab, phase2BossSpawnTransform.position, phase2BossSpawnTransform.rotation);

            yield return new WaitForSeconds(2);
            Destroy(phase1to2Bridge);
        }

        #endregion
    }
}
