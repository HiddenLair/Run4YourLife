using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.UI;

using Run4YourLife.Player;
using Run4YourLife.Utils;

using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class BossFightPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.BossFight; } }

        #region Editor variables

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private Transform m_bossFightStartingCameraPositionDebug;

        [SerializeField]
        private float m_timeOfPhase;

        [SerializeField]
        private Tiling m_backgroundTiling;

        [SerializeField]
        private Transform[] phase2Spawns;

        #endregion

        #region Member variables

        private PlayerSpawner m_playerSpawner;

        private GameObject m_uiManager;


        #endregion

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            m_uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_uiManager != null);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            m_playerSpawner.ActivateBoss();
            StartPhaseCommon();
        }

        void StartPhaseCommon()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Debug.Assert(boss != null);

            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            m_backgroundTiling.SetActive(false);
            ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnCountdownSetted(m_timeOfPhase));

            StartCoroutine(YieldHelper.SkipFrame(()=>MoveRunners()));           
            StartCoroutine(YieldHelper.WaitForSeconds(StartNextPhase, m_timeOfPhase));
        }

        private void MoveRunners()
        {
            for (int i = 0; i < GameplayPlayerManager.Instance.Runners.Count; i++)
            {
                GameObject runner = GameplayPlayerManager.Instance.Runners[i];
                runner.transform.position = phase2Spawns[i].position;
                RunnerCharacterController runnerCharacterController = runner.GetComponent<RunnerCharacterController>();
                runnerCharacterController.CheckOutScreen = true;
            }
        }

        private void StartNextPhase()
        {
            GameManager.Instance.EndExecutingPhaseAndStartPhase(GamePhase.BossFightRockTransition);
        }

        public override void EndPhase()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Destroy(boss);

            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerCharacterController runnerCharacterController = runner.GetComponent<RunnerCharacterController>(); 
                runnerCharacterController.CheckOutScreen = false; 
            }
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_playerSpawner.ActivatePlayers();
            m_virtualCamera.transform.position = m_bossFightStartingCameraPositionDebug.position;

            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            StopAllCoroutines();

            GameplayPlayerManager.Instance.DebugClearAllPlayers();

            EndPhaseCommon();
        }

        #endregion
    }
}
