using UnityEngine;
using Cinemachine;

using Run4YourLife.Player.Runner;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;

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
        private Transform m_bossFightStartingCameraPositionDebug;

        [SerializeField]
        private Transform[] m_runnerSpawns;

        [SerializeField]
        private AudioClip m_phaseMusic;


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

            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

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
            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerController runnerCharacterController = runner.GetComponent<RunnerController>();
                runnerCharacterController.CheckOutScreen = false;
            }
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            AudioManager.Instance.PlayMusic(m_phaseMusic);

            m_playerSpawner.ActivateBoss();
            m_playerSpawner.ActivatePlayers();
            m_virtualCamera.transform.position = m_bossFightStartingCameraPositionDebug.position;

            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            GameplayPlayerManager.Instance.DebugClearPlayers();
            m_bossFightGemManager.StopGemMinigame();

            EndPhaseCommon();
        }

        #endregion
    }
}
