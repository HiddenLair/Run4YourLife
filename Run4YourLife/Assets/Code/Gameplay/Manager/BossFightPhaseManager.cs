using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.UI;

using Run4YourLife.Player;

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

            m_uiManager = GameObject.FindGameObjectWithTag("UI");

            RegisterPhase(GamePhase.BossFight);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            GameObject boss = m_playerSpawner.InstantiateBossPlayer();
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);

            ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnCountdownSetted(m_timeOfPhase));
            StartCoroutine(StartNextPhaseInSeconds(m_timeOfPhase));
            m_triggerToPhase.SetActive(false);
            m_backgroundTiling.SetActive(false);
        }

        IEnumerator StartNextPhaseInSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            FindObjectOfType<GameManager>().EndExecutingPhaseAndStartPhase(GamePhase.TransitionToHardMoveHorizontal);
        }

        public override void EndPhase()
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            Destroy(boss);
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_virtualCamera.transform.position = m_bossFightStartingCameraPositionDebug.position;

            GameObject[] players = m_playerSpawner.InstantiatePlayers();

            GameObject boss = players.Where(x => x.CompareTag("Boss")).First();
            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);
            ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnCountdownSetted(m_timeOfPhase));
            StartCoroutine(StartNextPhaseInSeconds(m_timeOfPhase));
            m_triggerToPhase.SetActive(false);
            m_backgroundTiling.SetActive(false);
        }

        public override void DebugEndPhase()
        {
            GameObject boss = FindObjectOfType<Boss2>().gameObject;
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
            StopAllCoroutines();
        }

        #endregion
    }
}
