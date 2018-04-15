using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement {
    public interface IGameplayPlayerEvents : IEventSystemHandler
    {
        void OnRunnerDeath(GameObject player);
        void OnRunnerReviveRequest(Vector3 position);
    }

    public class GameplayPlayerManager : MonoBehaviour, IGameplayPlayerEvents {

        #region Editor

        [SerializeField]
        [Tooltip("All the runners that can be used in the game")]
        private GameObject[] m_sceneRunners;

        [SerializeField]
        [Tooltip("All the phase bosses that can be used in the game")]
        private List<GameObject> m_sceneBosses;

        [SerializeField]
        [Tooltip("Event that will be fired when all the players have died")]
        private UnityEvent m_onAllRunnersDied;

        [SerializeField]
        [Tooltip("Event that will be fired when a playerIsRevived")]
        private UnityEvent<GameObject> m_onPlayerRevived;

        #endregion

        #region Properties

        public GameObject Boss { get { return m_boss; } }
        public List<GameObject> Runners { get { return m_runners; } }
        public List<GameObject> RunnersAlive { get { return m_runnersAlive; } }
        public Queue<PlayerDefinition> DeadRunners { get { return m_deadRunners; } }
        public Dictionary<PlayerDefinition, GameObject> RunnerGameObject { get { return m_runnerGameObject; } }

        public PlayerDefinition BossPlayerDefinition { get { return m_bossPlayerDefinition; } }
        public List<PlayerDefinition> RunnerPlayerDefinitions { get { return m_runnerPlayerDefinitions; } }
        public int PlayerCount { get { return m_runnerPlayerDefinitions.Count + 1; } }

        #endregion

        #region Private Members

        private PlayerManager m_playerManager;

        private GameObject m_boss;
        private List<GameObject> m_runners = new List<GameObject>();
        private List<GameObject> m_runnersAlive = new List<GameObject>();
        private Queue<PlayerDefinition> m_deadRunners = new Queue<PlayerDefinition>();
        private Dictionary<PlayerDefinition, GameObject> m_runnerGameObject = new Dictionary<PlayerDefinition, GameObject>();

        private PlayerDefinition m_bossPlayerDefinition;
        private List<PlayerDefinition> m_runnerPlayerDefinitions = new List<PlayerDefinition>();

        #endregion

        #region Initialize

        private void Awake()
        {
            GameManager gameManager = GetComponent<GameManager>();
            Debug.Assert(gameManager != null);
            gameManager.onGamePhaseChanged.AddListener(OnGamePhaseChanged);

            m_playerManager = GetOrCreateDefaultPlayerManagerIfNoneIsAviable();
            InitializePlayers();
        }

        private PlayerManager GetOrCreateDefaultPlayerManagerIfNoneIsAviable()
        {
            //TODO if no playermanager is found, create default player manager
            //useful for debug opening the scene+
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager == null)
            {
                playerManager = gameObject.AddComponent<PlayerManager>();
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Purple,
                    ID = 1,
                    inputDevice = new Input.InputDevice(1),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Green,
                    ID = 2,
                    inputDevice = new Input.InputDevice(2),
                    IsBoss = true
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Orange,
                    ID = 3,
                    inputDevice = new Input.InputDevice(3),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Orange,
                    ID = 4,
                    inputDevice = new Input.InputDevice(4),
                    IsBoss = false
                });
            }
            return playerManager;
        }


        private void InitializePlayers()
        {
            foreach (PlayerDefinition playerDefinition in m_playerManager.GetPlayers())
            {
                if(playerDefinition.IsBoss)
                {
                    InitializeBoss(playerDefinition);
                } else
                {
                    InitializeRunner(playerDefinition);
                }
            }
        }

        private void InitializeRunner(PlayerDefinition playerDefinition)
        {
            m_runnerPlayerDefinitions.Add(playerDefinition);

            GameObject runner = GetRunnerForPlayer(playerDefinition);
            OnPlayerDefinitionChanged(runner, playerDefinition);


            if(!m_runnersAlive.Contains(runner))
            {
                m_runners.Add(runner);
            }

            m_runnerGameObject[playerDefinition] = runner;
        }

        private GameObject GetRunnerForPlayer(PlayerDefinition playerDefinition)
        {
            return m_sceneRunners[playerDefinition.ID - 1];
        }

        private void InitializeBoss(PlayerDefinition playerDefinition)
        {
            m_bossPlayerDefinition = playerDefinition;

            foreach (GameObject boss in m_sceneBosses)
            {
                OnPlayerDefinitionChanged(boss, playerDefinition);
            }
        }

        public void OnPlayerDefinitionChanged(GameObject player, PlayerDefinition playerDefinition)
        {
            player.SetActive(true);
            ExecuteEvents.Execute<IPlayerDefinitionEvents>(player, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
            player.SetActive(false);
        }

        #endregion

        public void OnRunnerDeath(GameObject runner)
        {
            m_runnersAlive.Remove(runner);
            
            PlayerDefinition playerDefinition = runner.GetComponent<PlayerInstance>().PlayerDefinition;
            m_deadRunners.Enqueue(playerDefinition);

            runner.SetActive(false);

            if (m_runnersAlive.Count == 0)
            {
                m_onAllRunnersDied.Invoke();
            }
        }

        public void OnRunnerReviveRequest(Vector3 position)
        {
            if(m_deadRunners.Count > 0)
            { 
                PlayerDefinition playerDefinition = m_deadRunners.Dequeue();
                GameObject runner = ActivateRunner(playerDefinition, position);
                m_onPlayerRevived.Invoke(runner);
            }
        }

        public GameObject ActivateRunner(PlayerDefinition playerDefinition, Vector3 position)
        {
            GameObject runner = m_runnerGameObject[playerDefinition];

            runner.transform.position = position;
            runner.SetActive(true);
            m_runnersAlive.Add(runner);
            return runner;
        }

        public GameObject ActivateBoss(GamePhase gamePhase, Vector3 position)
        {
            if(m_boss != null)
            {
                m_boss.SetActive(false);
            }

            m_boss = GetBossForPhase(gamePhase);
            m_boss.transform.position = position;
            m_boss.SetActive(true);
            return m_boss;
        }

        private GameObject GetBossForPhase(GamePhase gamePhase)
        {
            switch(gamePhase)
            {
                case GamePhase.EasyMoveHorizontal:
                case GamePhase.TransitionToEasyMoveHorizontal:
                    return m_sceneBosses[0];
                case GamePhase.TransitionToBossFight:
                case GamePhase.BossFight:
                    return m_sceneBosses[1];
                case GamePhase.TransitionToHardMoveHorizontal:
                case GamePhase.HardMoveHorizontal:
                    return m_sceneBosses[2];
                default:
                    Debug.LogError("You are trying to get a boss for a phase that has no boss");
                    return null;
            }
        }

        private void Update()
        {
            if (Debug.isDebugBuild)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.R))
                {
                    ReviveAllPlayers();
                }
            }
        }

        public void OnGamePhaseChanged(GamePhase gamePhase)
        {
            if(gamePhase.Equals(GamePhase.TransitionToBossFight) || gamePhase.Equals(GamePhase.TransitionToHardMoveHorizontal))
            {
                ReviveAllPlayers(); 
            }
        }

        public void ReviveAllPlayers()
        {
            while(m_deadRunners.Count > 0)
            {
                Vector3 position = GetRandomSpawnPosition();
                OnRunnerReviveRequest(position);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 position = Camera.main.transform.position;

            position.x += Random.Range(-3, 6);
            position.y += Random.Range(-2, 5);
            position.z = 0;

            return position;
        }

        public void DebugClearAllPlayers()
        {
            m_boss.SetActive(false);
            m_boss = null;
            foreach(GameObject runner in m_runnersAlive)
            {
                runner.SetActive(false);
            }

            m_deadRunners.Clear();
        }
    }
}
