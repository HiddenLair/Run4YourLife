using UnityEngine;
using Cinemachine;

using Run4YourLife.Player.Runner;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;
using System.Collections.Generic;
using Run4YourLife.Player.Boss;

namespace Run4YourLife.GameManagement
{
    [RequireComponent(typeof(BossFightGemManager))]
    public class BossFightPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.BossFight; } }

        #region Editor variables

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private Transform[] m_runnerSpawns;

        [SerializeField]
        private AudioClip m_phaseMusic;

        [SerializeField]
        private GameObject[] m_dynamicGameObjectsPrefabToDisableOnPhaseEnd;


        #endregion

        #region Member variables

        private PlayerSpawner m_playerSpawner;

        private BossFightGemManager m_bossFightGemManager;

        #endregion

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            m_bossFightGemManager = GetComponent<BossFightGemManager>();
            Debug.Assert(m_playerSpawner != null);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            StartPhaseCommon();
        }

        void StartPhaseCommon()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Debug.Assert(boss != null);

            boss.GetComponent<BossControllerPhase2>().enabled = true;

            m_bossFightGemManager.StartGemMinigame();
        }

        public void StartNextPhase()
        {
            GameManager.Instance.ChangeGamePhase(GamePhase.TransitionPhase2End);
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Debug.Assert(boss != null);
            boss.GetComponent<BossControllerPhase2>().enabled = false;

            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerController runnerCharacterController = runner.GetComponent<RunnerController>();
                runnerCharacterController.CheckOutScreen = false;
            }

            List<GameObject> activeInstances = new List<GameObject>();
            foreach (GameObject prefab in m_dynamicGameObjectsPrefabToDisableOnPhaseEnd)
            {
                DynamicObjectsManager.Instance.GameObjectPool.GetActiveNonAlloc(prefab, ref activeInstances);
                foreach (GameObject instance in activeInstances)
                {
                    instance.SetActive(false);
                }
            }
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            AudioManager.Instance.PlayMusic(m_phaseMusic);

            m_playerSpawner.ActivateBoss();
            m_playerSpawner.ActivatePlayers();
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            EndPhaseCommon();

            GameplayPlayerManager.Instance.DebugClearPlayers();
            m_bossFightGemManager.StopGemMinigame();
        }

        #endregion
    }
}
