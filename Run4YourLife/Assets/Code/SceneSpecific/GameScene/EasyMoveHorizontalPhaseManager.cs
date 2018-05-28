using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

using Run4YourLife.Player;
using Run4YourLife.UI;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.GameManagement
{
    public class EasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.EasyMoveHorizontal; } }

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        private PlayerSpawner m_playerSpawner;
        private GameObject m_ui;

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            StartPhaseCommon();
        }

        void StartPhaseCommon()
        {
            AudioManager.Instance.PlayMusic(AudioManager.Music.GameScene);

            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Debug.Assert(boss != null);

            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            boss.GetComponent<BossPathWalker>().m_position = 0;

            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerCharacterController runnerCharacterController = runner.GetComponent<RunnerCharacterController>();
                runnerCharacterController.CheckOutScreen = true;
            }

            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a,b) => a.ShowCrossHair());
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        void EndPhaseCommon()
        {
            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerCharacterController runnerCharacterController = runner.GetComponent<RunnerCharacterController>();
                runnerCharacterController.CheckOutScreen = false;
            }
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a,b) => a.HideCrossHair());
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_playerSpawner.ActivatePlayers();
            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            GameplayPlayerManager.Instance.DebugClearAllPlayers();
            EndPhaseCommon();
        }

        #endregion
    }
}