using System.Collections.Generic;

using UnityEngine;
using Cinemachine;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class EasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.EasyMoveHorizontal; } }

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

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

            List<GameObject> runners = GameplayPlayerManager.Instance.Runners;

            foreach (GameObject g in runners)
            {
                RunnerCharacterController tempController = g.GetComponent<RunnerCharacterController>();
                tempController.SetLimitScreenRight(true);
                tempController.SetLimitScreenLeft(true);
                tempController.SetCheckOutScreen(true);
            }
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        void EndPhaseCommon()
        {
            List<GameObject> runners = GameplayPlayerManager.Instance.Runners;

            foreach (GameObject g in runners)
            {
                RunnerCharacterController tempController = g.GetComponent<RunnerCharacterController>();
                tempController.SetLimitScreenRight(false);
                tempController.SetLimitScreenLeft(false);
                tempController.SetCheckOutScreen(false);
            }
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