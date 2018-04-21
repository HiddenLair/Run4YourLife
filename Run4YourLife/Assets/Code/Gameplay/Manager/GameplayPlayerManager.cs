using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;
using Run4YourLife.Input;

namespace Run4YourLife.GameManagement {
    public interface IGameplayPlayerEvents : IEventSystemHandler
    {
        void OnRunnerDeath(GameObject player);
        void OnRunnerReviveRequest(Vector3 position);
    }

    [RequireComponent(typeof(GameManager))]
    [RequireComponent(typeof(RunnerPrefabManager))]
    public class GameplayPlayerManager : SingletonMonoBehaviour<GameplayPlayerManager>, IGameplayPlayerEvents {

        #region Editor

        [SerializeField]
        [Tooltip("All the phase bosses that can be used in the game")]
        private List<GameObject> m_sceneBosses;

        [SerializeField]
        [Tooltip("Event that will be fired when all the players have died")]
        private UnityEvent m_onAllRunnersDied;

        [SerializeField]
        [Tooltip("Event that will be fired when a playerIsRevived")]
        private UnityEvent<GameObject> m_onPlayerRevived;

        [SerializeField]
        private GameObject[] runnerSlot;

        #endregion

        #region Properties

        public GameObject Boss { get { return m_boss; } }
        public List<GameObject> Runners { get { return m_runners; } }
        public List<GameObject> RunnersAlive { get { return m_runnersAlive; } }
        public Queue<PlayerHandle> DeadRunners { get { return m_deadRunners; } }
        public Dictionary<PlayerHandle, GameObject> RunnerGameObject { get { return m_runnerGameObject; } }

        public PlayerHandle BossPlayerDefinition { get { return m_bossPlayerDefinition; } }
        public List<PlayerHandle> RunnerPlayerHandles { get { return m_runnerPlayerDefinitions; } }
        public int PlayerCount { get { return m_runnerPlayerDefinitions.Count + 1; } }

        #endregion

        #region Private Members

        private PlayerManager m_playerManager;

        private GameObject m_boss;
        private List<GameObject> m_runners = new List<GameObject>();
        private List<GameObject> m_runnersAlive = new List<GameObject>();
        private Queue<PlayerHandle> m_deadRunners = new Queue<PlayerHandle>();
        private Dictionary<PlayerHandle, GameObject> m_runnerGameObject = new Dictionary<PlayerHandle, GameObject>();

        private PlayerHandle m_bossPlayerDefinition;
        private List<PlayerHandle> m_runnerPlayerDefinitions = new List<PlayerHandle>();

        private RunnerPrefabManager m_runnerPrefabManager;

        private int runnerIndex = 0;

        #endregion

        #region Initialize

        private void Awake()
        {
            GameManager gameManager = GetComponent<GameManager>();
            gameManager.onGamePhaseChanged.AddListener(OnGamePhaseChanged);

            m_runnerPrefabManager = GetComponent<RunnerPrefabManager>();
        }

        private void Start()
        {
            InitializePlayers();
        }

        private void InitializePlayers()
        {
            if(PlayerManager.Instance.GetPlayers().Count == 0)
            {
                CreateDebugPlayers();
            }

            foreach (PlayerHandle playerDefinition in PlayerManager.Instance.GetPlayers())
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

        private void CreateDebugPlayers()
        {
            Debug.Log("Creating debug player handles for game");

            PlayerHandle[] playerHandles = new PlayerHandle[]
            {
                new PlayerHandle() {
                    CharacterType = CharacterType.Purple,
                    ID = 1,
                    inputDevice = InputDeviceManager.Instance.InputDevices[1],
                    IsBoss = false
                },
                new PlayerHandle() {
                    CharacterType = CharacterType.Green,
                    ID = 2,
                    inputDevice = InputDeviceManager.Instance.InputDevices[0],
                    IsBoss = true
                },
                new PlayerHandle() {
                    CharacterType = CharacterType.Orange,
                    ID = 3,
                    inputDevice = InputDeviceManager.Instance.InputDevices[2],
                    IsBoss = false
                },
                new PlayerHandle() {
                    CharacterType = CharacterType.Green,
                    ID = 4,
                    inputDevice = InputDeviceManager.Instance.InputDevices[3],
                    IsBoss = false
                }
            };

            foreach (PlayerHandle playerHandle in playerHandles)
            {
                PlayerManager.Instance.AddPlayer(playerHandle);
            }
        }

        private void InitializeRunner(PlayerHandle playerDefinition)
        {
            m_runnerPlayerDefinitions.Add(playerDefinition);

            GameObject runner = Instantiate(GetRunnerForPlayer(playerDefinition), runnerSlot[runnerIndex].transform, false);

            runnerIndex++;
            runner.SetActive(false);
            OnPlayerDefinitionChanged(runner, playerDefinition);

            if(!m_runnersAlive.Contains(runner))
            {
                m_runners.Add(runner);
            }

            m_runnerGameObject[playerDefinition] = runner;
        }

        private GameObject GetRunnerForPlayer(PlayerHandle playerDefinition)
        {
            return m_runnerPrefabManager.Get(playerDefinition.CharacterType);
        }

        private void InitializeBoss(PlayerHandle playerDefinition)
        {
            m_bossPlayerDefinition = playerDefinition;

            foreach (GameObject boss in m_sceneBosses)
            {
                OnPlayerDefinitionChanged(boss, playerDefinition);
            }
        }

        public void OnPlayerDefinitionChanged(GameObject player, PlayerHandle playerDefinition)
        {
            player.SetActive(true);
            ExecuteEvents.Execute<IPlayerHandleEvent>(player, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
            player.SetActive(false);
        }

        #endregion

        public void OnRunnerDeath(GameObject runner)
        {
            m_runnersAlive.Remove(runner);
            
            PlayerHandle playerDefinition = runner.GetComponent<PlayerInstance>().PlayerHandle;

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
                PlayerHandle playerDefinition = m_deadRunners.Dequeue();
                GameObject runner = ActivateRunner(playerDefinition, position);
                m_onPlayerRevived.Invoke(runner);
            }
        }

        public GameObject ActivateRunner(PlayerHandle playerDefinition, Vector3 position)
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
            for(int i = 0; i< m_runnersAlive.Count;i++)
            {
                m_runnersAlive[i].SetActive(false);
            }

            m_deadRunners.Clear();
        }
    }
}
