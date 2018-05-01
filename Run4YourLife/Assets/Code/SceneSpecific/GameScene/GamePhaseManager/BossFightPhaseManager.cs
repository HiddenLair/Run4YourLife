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
            UnityEngine.Debug.Assert(m_playerSpawner != null);

            m_uiManager = GameObject.FindGameObjectWithTag(Tags.UI);

            RegisterPhase(GamePhase.BossFight);
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
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);

            m_backgroundTiling.SetActive(false);
            ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnCountdownSetted(m_timeOfPhase));

            StartCoroutine(YieldHelper.SkipFrame(()=>MoveRunners()));           
            StartCoroutine(YieldHelper.WaitForSeconds(StartNextPhase, m_timeOfPhase));
        }

        private void MoveRunners()
        {
            for (int i = 0; i < GameplayPlayerManager.Instance.Runners.Count; i++)
            {
                GameplayPlayerManager.Instance.Runners[i].transform.position = phase2Spawns[i].position;
                RunnerCharacterController tempController = GameplayPlayerManager.Instance.Runners[i].GetComponent<RunnerCharacterController>();
                tempController.SetLimitScreenLeft(true);
                tempController.SetLimitScreenRight(true);
                tempController.SetCheckOutScreen(true);
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
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);

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
