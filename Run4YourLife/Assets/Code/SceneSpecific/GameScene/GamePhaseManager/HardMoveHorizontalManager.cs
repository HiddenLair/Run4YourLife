using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

using Run4YourLife.Utils;
using Run4YourLife.Player;
using System;
using System.Collections;

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
        private Tiling m_background;

        [SerializeField]
        private Material m_newBackgroundMat;

        [SerializeField]
        private Transform[] m_runnerSpawns;

        #endregion

        #region Member Variables

        private PlayerSpawner m_playerSpawner;
        private GameObject m_ui;

        #endregion

        #region Regular Execution

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);
        }

        #endregion

        public override void StartPhase()
        {
            m_checkPointManager.gameObject.SetActive(true);
            m_playerSpawner.ActivateBoss();
            StartPhaseCommon();
        }

        private void StartPhaseCommon()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            boss.GetComponent<BossPathWalker>().m_position = 0;

            StartCoroutine(YieldHelper.SkipFrame(() => MoveRunners()));

            m_background.SetActive(true);

            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a,b) => a.ShowCrossHair());
        }

        private void MoveRunners()
        {
            for (int i = 0; i < GameplayPlayerManager.Instance.Runners.Count; i++)
            {
                GameObject runner = GameplayPlayerManager.Instance.Runners[i];
                runner.transform.position = m_runnerSpawns[i].position;
                RunnerController runnerCharacterController = runner.GetComponent<RunnerController>();
                runnerCharacterController.CheckOutScreen = true;
            }
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
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a,b) => a.HideCrossHair());
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_checkPointManager.gameObject.SetActive(true);
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

