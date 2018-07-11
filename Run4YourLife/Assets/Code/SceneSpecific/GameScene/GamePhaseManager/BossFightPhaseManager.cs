﻿using UnityEngine;
using Run4YourLife.Player;
using Run4YourLife.Utils;
using Cinemachine;

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
            StartCoroutine(YieldHelper.WaitForSeconds(() => m_bossFightGemManager.ActivateNextGem(), 3));
        }

        void StartPhaseCommon()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Debug.Assert(boss != null);

            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);       
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
            m_playerSpawner.ActivateBoss();
            m_playerSpawner.ActivatePlayers();
            m_virtualCamera.transform.position = m_bossFightStartingCameraPositionDebug.position;

            StartPhaseCommon();
            StartCoroutine(YieldHelper.WaitForSeconds(() => m_bossFightGemManager.ActivateNextGem(), 3));
        }

        public override void DebugEndPhase()
        {
            StopAllCoroutines();

            GameplayPlayerManager.Instance.DebugClearPlayers();

            EndPhaseCommon();
        }

        #endregion
    }
}
