using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class Phase2Manager : GamePhaseManager
    {
        #region Member variables
        [SerializeField]
        private Transform[] m_debugRunnerSpawn;

        [SerializeField]
        private Transform m_bossSpawn;

        [SerializeField]
        private GameObject m_bossPrefab;
        
        [SerializeField]
        private GameObject m_phase1to2Bridge;

        [SerializeField]
        private GameObject m_phase2StartTrigger;

        #endregion

        #region Debug variables

        [SerializeField]
        private GameObject m_debugBlueRunnerPrefab;

        [SerializeField]
        private GameObject m_debugRedRunnerPrefab;

        [SerializeField]
        private GameObject m_debugGreenRunnerPrefab;

        [SerializeField]
        private GameObject m_debugOrangeRunnerPrefab;

        #endregion

        #region Initialization

        private void Awake()
        {
            RegisterPhase(GamePhase.Phase2);
        }

        #endregion

        #region Regular Execution

        public override void EndPhase()
        {
            GameObject boss = FindObjectOfType<Boss2>().gameObject;
            Destroy(boss);
        }

        public override void StartPhase()
        {
            StartCoroutine(StartPhase2());
        }

        private IEnumerator StartPhase2()
        {
            m_phase2StartTrigger.SetActive(false);
            GameObject boss = Instantiate(m_bossPrefab, m_bossSpawn.position, m_bossSpawn.rotation);
            Camera.main.GetComponent<CameraBossFollow>().boss = boss.transform; // TODO: Temporal camera attachment


            yield return new WaitForSeconds(2);
            m_phase1to2Bridge.SetActive(false);
        }

        #endregion

        #region Debug Execution

        public override void DebugEndPhase()
        {
            GameObject boss = FindObjectOfType<Boss2>().gameObject;
            Debug.Assert(boss != null);
            Destroy(boss);

            PlayerCharacterController[] players = FindObjectsOfType<PlayerCharacterController>();
            foreach (PlayerCharacterController player in players)
            {
                Destroy(player.gameObject);
            }
        }

        public override void DebugStartPhase()
        {
            m_phase1to2Bridge.SetActive(false);
            m_phase2StartTrigger.SetActive(false);

            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            InstantiatePlayers(playerManager.GetPlayers());
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
                instance = Instantiate(m_bossPrefab, m_bossSpawn.position, m_bossSpawn.rotation);
                Camera.main.GetComponent<CameraBossFollow>().boss = instance.transform; // TODO: Temporal camera attachment
            }
            else
            {
                GameObject runnerPrefab = null;
                switch (playerDefinition.CharacterType)
                {
                    case CharacterType.Blue:
                        runnerPrefab = m_debugBlueRunnerPrefab;
                        break;
                    case CharacterType.Green:
                        runnerPrefab = m_debugGreenRunnerPrefab;
                        break;
                    case CharacterType.Orange:
                        runnerPrefab = m_debugOrangeRunnerPrefab;
                        break;
                    case CharacterType.Red:
                        runnerPrefab = m_debugRedRunnerPrefab;
                        break;
                }
                // TODO implementation of spawn that does not require 3 transforms instead of 4
                Transform spawn = m_debugRunnerSpawn[playerDefinition.ID - 1];
                instance = Instantiate(runnerPrefab, spawn.position, spawn.rotation);
            }
            return instance;
        }

        #endregion
    }
}
