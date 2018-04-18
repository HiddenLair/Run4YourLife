using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Run4YourLife.Utils;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class HardMoveHorizontalManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private CheckPointManager m_checkPointManager;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private GameObject m_background;

        [SerializeField]
        private Material m_newBackgroundMat;

        [SerializeField]
        private Transform[] phase2Spawns;

        #endregion

        #region Member Variables

        private PlayerSpawner m_playerSpawner;
        private GameplayPlayerManager m_gameplayPlayerManager;

        #endregion

        #region Regular Execution

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);

            RegisterPhase(GamePhase.HardMoveHorizontal);
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

            GameObject boss = m_gameplayPlayerManager.Boss;
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);

            StartCoroutine(YieldHelper.SkipFrame(() => MoveRunners()));

            m_background.GetComponent<Renderer>().material = m_newBackgroundMat;
            m_background.GetComponent<Tiling>().mat = m_newBackgroundMat;
            m_background.GetComponent<Tiling>().SetActive(true);
        }

        private void MoveRunners()
        {
            for (int i = 0; i < m_gameplayPlayerManager.Runners.Count; i++)
            {
                m_gameplayPlayerManager.Runners[i].transform.position = phase2Spawns[i].position;
            }
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            m_checkPointManager.gameObject.SetActive(false);

            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);
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
            m_gameplayPlayerManager.DebugClearAllPlayers();
            EndPhaseCommon();
        }

        #endregion
    }
}

