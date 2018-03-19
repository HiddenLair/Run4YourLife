using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.UI;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class BossFightPhaseManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private CameraTargetCentered m_cameraTargetCentered;

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
            m_cameraTargetCentered.m_target = boss.transform; // TODO: Temporal camera attachment
            m_cameraTargetCentered.enabled = true;
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
            m_cameraTargetCentered.enabled = false;
            m_cameraTargetCentered.m_target = null;
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_cameraTargetCentered.transform.position = m_bossFightStartingCameraPositionDebug.position;

            GameObject[] players = m_playerSpawner.InstantiatePlayers();

            GameObject boss = players.Where(x => x.CompareTag("Boss")).First();
            m_cameraTargetCentered.m_target = boss.transform; // TODO: Temporal camera attachment
            m_cameraTargetCentered.enabled = true;
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

            PlayerCharacterController[] players = FindObjectsOfType<PlayerCharacterController>();
            foreach (PlayerCharacterController player in players)
            {
                Destroy(player.gameObject);
            }
            m_cameraTargetCentered.enabled = false;
            StopAllCoroutines();
        }

        #endregion
    }
}
