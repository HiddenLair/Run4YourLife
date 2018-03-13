using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class Phase1Manager : GamePhaseManager
    {
        #region Member variables

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

        [SerializeField]
        private GameObject m_phase1to2Bridge;

        [SerializeField]
        private GameObject m_phase2StartTrigger;

        [SerializeField]
        private CameraBossFollow m_cameraBossFollow;

        #endregion

        #region Initialization

        private void Awake()
        {
            RegisterPhase(GamePhase.Phase1);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            Debug.Assert(playerManager != null);

            InstantiatePlayers(playerManager.GetPlayers());

            m_cameraBossFollow.enabled = true;
            m_phase1to2Bridge.SetActive(true);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            //Boss should play exit animation
            GameObject boss = FindObjectOfType<Boss>().gameObject;
            Debug.Assert(boss != null);
            Destroy(boss);
            m_cameraBossFollow.enabled = false;
        }

        private void InstantiatePlayers(List<PlayerDefinition> playerDefinitions)
        {
            foreach (PlayerDefinition playerDefinition in playerDefinitions)
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

            if (playerDefinition.IsBoss)
            {
                instance = Instantiate(bossPrefab, spawnLocationBoss.position, spawnLocationBoss.rotation);
                m_cameraBossFollow.boss = instance.transform; // TODO: Temporal camera attachment
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

        #region Debug Execution

        public override void DebugEndPhase()
        {
            GameObject boss = FindObjectOfType<Boss>().gameObject;
            Debug.Assert(boss != null);
            Destroy(boss);

            PlayerCharacterController[] players = FindObjectsOfType<PlayerCharacterController>();
            foreach (PlayerCharacterController player in players)
            {
                Destroy(player.gameObject);
            }
            m_cameraBossFollow.enabled = false;
        }

        public override void DebugStartPhase()
        {
            StartPhase();
        }

        #endregion
    }
}