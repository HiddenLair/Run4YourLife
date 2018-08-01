using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

using Run4YourLife.Player.Runner;
using Run4YourLife.Player.Boss;
using Run4YourLife.UI;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.GameManagement
{
    public class EasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.EasyMoveHorizontal; } }

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private AudioClip m_phaseMusic;

        private PlayerSpawner m_playerSpawner;

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
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

            boss.GetComponent<BossPathWalker>().m_position = 0;

            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerController runnerCharacterController = runner.GetComponent<RunnerController>();
                runnerCharacterController.CheckOutScreen = true;
            }
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        void EndPhaseCommon()
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
            m_playerSpawner.ActivatePlayers();
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