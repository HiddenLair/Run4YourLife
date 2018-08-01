using UnityEngine;
using UnityEngine.EventSystems;

using Cinemachine;

using Run4YourLife.Utils;
using Run4YourLife.Player.Runner;
using Run4YourLife.Player.Boss;
using Run4YourLife.UI;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.GameManagement
{
    public class HardMoveHorizontalManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.HardMoveHorizontal; } }

        #region Editor variables

        [SerializeField]
        private BossPath m_checkPointManager;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private AudioClip m_phaseMusic;

        #endregion

        #region Member Variables

        private PlayerSpawner m_playerSpawner;

        #endregion

        #region Regular Execution

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);
        }

        #endregion

        public override void StartPhase()
        {
            m_checkPointManager.gameObject.SetActive(true);
            StartPhaseCommon();
        }

        private void StartPhaseCommon()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            boss.GetComponent<BossPathWalker>().m_position = 0;
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            m_checkPointManager.gameObject.SetActive(false);

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
            m_checkPointManager.gameObject.SetActive(true);
            m_playerSpawner.ActivatePlayers();
            m_playerSpawner.ActivateBoss();
            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            GameplayPlayerManager.Instance.DebugClearPlayers();
            EndPhaseCommon();
        }

        #endregion
    }
}

