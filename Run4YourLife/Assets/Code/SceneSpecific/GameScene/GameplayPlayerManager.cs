using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;
using Run4YourLife.InputManagement;
using Run4YourLife.Debugging;

namespace Run4YourLife.GameManagement {
    public interface IGameplayPlayerEvents : IEventSystemHandler
    {
        void OnRunnerDeath(PlayerHandle player, Vector3 position);
        GameObject OnRunnerRevive(PlayerHandle playerHandle, Vector3 position);
        GameObject OnRunnerActivate(PlayerHandle playerHandle, Vector3 position);
    }

    [System.Serializable]
    public class OnPlayerReviveEvent : UnityEvent<GameObject> { }

    [RequireComponent(typeof(GameManager))]
    [RequireComponent(typeof(RunnerPrefabManager))]
    public class GameplayPlayerManager : SingletonMonoBehaviour<GameplayPlayerManager>, IGameplayPlayerEvents {

        #region Editor

        [SerializeField]
        [Tooltip("All the phase bosses that can be used in the game")]
        private List<GameObject> m_sceneBosses;

        [SerializeField]
        [Tooltip("Objects in the hierarchy where the runners will be spawned at")]
        private Transform[] m_runnerSlot;

        #endregion

        #region Properties

        public GameObject Boss { get { return m_boss; } }
        
        public List<GameObject> Runners { get { return m_runners; } }
        public List<GameObject> RunnersAlive { get { return m_runnersAlive; } }
        public Dictionary<PlayerHandle, GameObject> RunnerGameObject { get { return m_runnerGameObject; } }

        public List<GameObject> Ghosts { get { return m_ghosts; } }
        public List<GameObject> GhostsAlive { get { return m_ghostsAlive; } }
        public Dictionary<PlayerHandle, GameObject> GhostGameObject { get { return m_ghostGameObject; } }

        #endregion

        #region Members

        private bool firstDeath = true;
        private GameObject m_boss;
        private List<GameObject> m_runners = new List<GameObject>();
        private List<GameObject> m_runnersAlive = new List<GameObject>();
        private Dictionary<PlayerHandle, GameObject> m_runnerGameObject = new Dictionary<PlayerHandle, GameObject>();

        private List<GameObject> m_ghosts = new List<GameObject>();
        private List<GameObject> m_ghostsAlive = new List<GameObject>();
        private Dictionary<PlayerHandle, GameObject> m_ghostGameObject = new Dictionary<PlayerHandle, GameObject>();

        private RunnerPrefabManager m_runnerPrefabManager;

        private IGameplayEvents m_gameplayEvents;
        private FirstKillManager m_firstKillManager;

        #endregion

        #region Initialize

        private void Awake()
        {
            m_runnerPrefabManager = GetComponent<RunnerPrefabManager>();
            m_gameplayEvents = GetComponent<IGameplayEvents>();
            m_firstKillManager = FindObjectOfType<FirstKillManager>();
        }

        private void Start()
        {
            InitializePlayers();
        }

        private void InitializePlayers()
        {
            if(PlayerManager.Instance.PlayerHandles.Count == 0)
            {
                CreateDebugPlayers();
            }

            int runnerIndex = 0;
            foreach (PlayerHandle playerHandle in PlayerManager.Instance.PlayerHandles)
            {
                if(playerHandle.IsBoss)
                {
                    InitializeBoss(playerHandle);
                } else
                {
                    InitializeRunner(playerHandle, runnerIndex);
                    InitializeRunnerGhost(playerHandle, runnerIndex);
                    runnerIndex++;
                }
            }
        }

        private void CreateDebugPlayers()
        {
            PlayerHandle[] playerHandles = new PlayerHandle[]
            {
                new PlayerHandle() {
                    CharacterType = CharacterType.Purple,
                    InputDevice = InputDeviceManager.Instance.InputDevices[0],
                    IsBoss = false
                },
                new PlayerHandle() {
                    CharacterType = CharacterType.Green,
                    InputDevice = InputDeviceManager.Instance.InputDevices[1],
                    IsBoss = true
                },
                new PlayerHandle() {
                    CharacterType = CharacterType.White,
                    InputDevice = InputDeviceManager.Instance.InputDevices[2],
                    IsBoss = false
                },
                new PlayerHandle() {
                    CharacterType = CharacterType.Red,
                    InputDevice = InputDeviceManager.Instance.InputDevices[3],
                    IsBoss = false
                }
            };

            foreach (PlayerHandle playerHandle in playerHandles)
            {
                PlayerManager.Instance.AddPlayer(playerHandle);
            }
        }

        private void InitializeRunner(PlayerHandle playerHandle, int runnerIndex)
        {
            GameObject runner = Instantiate(GetRunnerForPlayer(playerHandle), m_runnerSlot[runnerIndex], false);
            runner.SetActive(false);
            m_runners.Add(runner);

            OnplayerHandleChanged(runner, playerHandle);

            m_runnerGameObject[playerHandle] = runner;
        }

        private void InitializeRunnerGhost(PlayerHandle playerHandle, int runnerIndex)
        {
            GameObject ghostRunner = Instantiate(GetGhostForPlayer(playerHandle), m_runnerSlot[runnerIndex], false);
            ghostRunner.SetActive(false);
            m_ghosts.Add(ghostRunner);

            OnplayerHandleChanged(ghostRunner, playerHandle);

            m_ghostGameObject[playerHandle] = ghostRunner;
        }

        private GameObject GetRunnerForPlayer(PlayerHandle playerHandle)
        {
            return m_runnerPrefabManager.GetRunner(playerHandle.CharacterType);
        }

        private GameObject GetGhostForPlayer(PlayerHandle playerHandle)
        {
            return m_runnerPrefabManager.GetGhost(playerHandle.CharacterType);
        }

        private void InitializeBoss(PlayerHandle playerHandle)
        {
            foreach (GameObject boss in m_sceneBosses)
            {
                OnplayerHandleChanged(boss, playerHandle);
            }
        }

        public void OnplayerHandleChanged(GameObject player, PlayerHandle playerHandle)
        {
            foreach(IPlayerHandleEvent playerHandleEvent in player.GetComponents<IPlayerHandleEvent>())
            {
                playerHandleEvent.OnPlayerHandleChanged(playerHandle);
            }
        }

        #endregion

        public void OnRunnerDeath(PlayerHandle playerHandle, Vector3 position)
        {
            DeactivateRunner(playerHandle);
            ActivateRunnerGhost(playerHandle, position);

            if (m_runnerGameObject[playerHandle].GetComponent<RunnerController>().GetReviveMode()) // TODO: this means if it is invincible naming is bad
            {
                OnRunnerRevive(playerHandle, GetRandomSpawnPosition());
            }
            else if (m_runnersAlive.Count == 0)
            {
                m_gameplayEvents.EndGame_BossWin();
            }
            else if (firstDeath && m_firstKillManager != null)//If we are not alone in the tutorial, we will show the revive text
            {
                m_firstKillManager.ShowReviveInfo();
                firstDeath = false;
            }
        }

        public GameObject OnRunnerRevive(PlayerHandle playerHandle, Vector3 position)
        {
            DeactivateGhost(playerHandle);
            return ActivateRunner(playerHandle, position, true);
        }

        public GameObject OnRunnerActivate(PlayerHandle playerHandle, Vector3 position) 
        {
            DeactivateGhost(playerHandle);
            return ActivateRunner(playerHandle, position, false);
        }

        private GameObject ActivateRunner(PlayerHandle playerHandle, Vector3 position, bool revived = false)
        {
            GameObject runner = m_runnerGameObject[playerHandle];
            runner.transform.position = position;
            m_runnersAlive.Add(runner);
            runner.SetActive(true);

            if(revived)
            {
                runner.GetComponent<RunnerController>().RecentlyRevived();
            }

            return runner;
        }

        private GameObject ActivateRunnerGhost(PlayerHandle playerhandle, Vector3 position)
        {
            GameObject ghost = m_ghostGameObject[playerhandle];
            ghost.transform.position = position;
            m_ghostsAlive.Add(ghost);
            ghost.SetActive(true);
            return ghost;
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

        private void DeactivateRunner(PlayerHandle playerHandle)
        {
            GameObject runner = m_runnerGameObject[playerHandle];
            m_runnersAlive.Remove(runner);
            runner.SetActive(false);
        }

        private void DeactivateGhost(PlayerHandle playerHandle)
        {
            GameObject ghost = m_ghostGameObject[playerHandle];
            m_ghostsAlive.Remove(ghost);
            ghost.SetActive(false);
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
            if(DebugSystemManagerHelper.DebuggingToolsEnabled())
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    DebugReviveAllRunners();
                }
            }
        }

        public void DebugReviveAllRunners()
        {
            while(m_ghostsAlive.Count > 0)
            {
                GameObject ghost = m_ghostsAlive[m_ghostsAlive.Count - 1];
                PlayerHandle ghostPlayerHandle = ghost.GetComponent<PlayerInstance>().PlayerHandle;
                Vector3 position = GetRandomSpawnPosition();
                OnRunnerRevive(ghostPlayerHandle, position);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 position = CameraManager.Instance.MainCamera.transform.position;

            position.x += Random.Range(-3, 6);
            position.y += Random.Range(-2, 5);
            position.z = 0;

            return position;
        }

        public void DebugClearPlayers()
        {
            DebugClearBoss();
            DebugClearRunners();
        }

        public void DebugClearBoss()
        {
            m_boss.SetActive(false);
            m_boss = null;
        }

        public void DebugClearRunners()
        {
            foreach(GameObject runner in m_runnersAlive)
            {
                runner.SetActive(false);
            }

            foreach(GameObject ghost in m_ghostsAlive)
            {
                ghost.SetActive(false);
            }

            m_runnersAlive.Clear();
            m_ghostsAlive.Clear();
        }

        public void DebugActivateRunners()
        {
            foreach(GameObject runner in m_runners)
            {
                m_runnersAlive.Add(runner);
                runner.transform.position = GetRandomSpawnPosition();
                runner.SetActive(true);
            }
        }
    }
}
