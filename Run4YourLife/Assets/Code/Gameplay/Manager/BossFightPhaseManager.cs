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
        private GameObject m_triggerToPhase;

        [SerializeField]
        private Tiling m_backgroundTiling;

        [SerializeField]
        private Transform[] phase2Spawns;

        #endregion

        #region Member variables

        private GameplayPlayerManager m_gameplayPlayerManager;
        private PlayerSpawner m_playerSpawner;

        private GameObject m_uiManager;


        #endregion

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);

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
            GameObject boss = m_gameplayPlayerManager.Boss;
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);

            StartCoroutine(YieldHelper.SkipFrame(()=>MoveRunners()));

            m_backgroundTiling.SetActive(false);

            ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnCountdownSetted(m_timeOfPhase));
            StartCoroutine(YieldHelper.WaitForSeconds(StartNextPhase, m_timeOfPhase));
        }

        private void MoveRunners()
        {
            for (int i = 0; i < m_gameplayPlayerManager.Runners.Count; i++)
            {
                m_gameplayPlayerManager.Runners[i].transform.position = phase2Spawns[i].position;
            }
        }

        private void StartNextPhase()
        {
            FindObjectOfType<GameManager>().EndExecutingPhaseAndStartPhase(GamePhase.BossFightRockTransition);
        }

        public override void EndPhase()
        {
            GameObject boss = m_gameplayPlayerManager.Boss;
            Destroy(boss);

            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);
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

            m_gameplayPlayerManager.DebugClearAllPlayers();

            EndPhaseCommon();
        }

        #endregion
    }
}
