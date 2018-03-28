using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            m_playerSpawner.InstantiateBossPlayer();
            StartPhaseCommon();
        }

        private void StartPhaseCommon()
        {

            GameObject boss = m_gameplayPlayerManager.Boss;
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);
            m_background.GetComponent<Renderer>().material = m_newBackgroundMat;
            m_background.GetComponent<Tiling>().mat = m_newBackgroundMat;
            m_background.GetComponent<Tiling>().SetActive(true);
        }

        public override void EndPhase()
        {
            GameObject boss = m_gameplayPlayerManager.Boss;
            Destroy(boss);
            m_gameplayPlayerManager.Boss = null;
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
            m_playerSpawner.InstantiatePlayers();

            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            m_gameplayPlayerManager.DebugDestroyAllPlayersAndClear();
            EndPhaseCommon();
        }

        #endregion
    }
}

