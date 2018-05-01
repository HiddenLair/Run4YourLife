using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Run4YourLife.Utils;
using Cinemachine;
using Run4YourLife.Player;

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

        #endregion

        #region Regular Execution

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            UnityEngine.Debug.Assert(m_playerSpawner != null);


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

            GameObject boss = GameplayPlayerManager.Instance.Boss;
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);

            StartCoroutine(YieldHelper.SkipFrame(() => MoveRunners()));

            m_background.GetComponent<Tiling>().SetActive(true);
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
            m_checkPointManager.gameObject.SetActive(true);
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

