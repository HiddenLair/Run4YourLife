using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class EasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private GameObject m_phase1to2Bridge;

        [SerializeField]
        private GameObject m_phase2StartTrigger;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private CheckPointManager m_checkPointManager;

        #endregion

        #region Member variables

        private PlayerSpawner m_playerSpawner;

        #endregion

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            RegisterPhase(GamePhase.EasyMoveHorizontal);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss"); //TODO make class with all tags and reference that
            Debug.Assert(boss != null);
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;

            m_virtualCamera.gameObject.SetActive(true);
            m_phase1to2Bridge.SetActive(true);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
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
            GameObject[] players = m_playerSpawner.InstantiatePlayers();

            GameObject boss = players.Where(x => x.CompareTag("Boss")).First();
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;

            m_virtualCamera.gameObject.SetActive(true);

            m_phase1to2Bridge.SetActive(true);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void DebugEndPhase()
        {
            m_checkPointManager.gameObject.SetActive(false);
            GameObject boss = FindObjectOfType<Boss>().gameObject;
            Debug.Assert(boss != null);
            Destroy(boss);

            RunnerCharacterController[] players = FindObjectsOfType<RunnerCharacterController>();
            foreach (RunnerCharacterController player in players)
            {
                Destroy(player.gameObject);
            }
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);
        }

        #endregion
    }
}