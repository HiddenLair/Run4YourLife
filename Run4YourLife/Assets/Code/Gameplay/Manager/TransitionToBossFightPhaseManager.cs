using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToBossFightPhaseManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private GameObject m_transitionStartTrigger;

        [SerializeField]
        private GameObject m_phase2StartTrigger;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private GameObject cameraHandlePrefab;

        [SerializeField]
        private Transform cameraHandleTarget;

        #endregion

        private GameObject cameraHandleInstance;

        private GameplayPlayerManager m_gameplayPlayerManager;

        #region Initialization

        private void Awake()
        {
            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);

            RegisterPhase(GamePhase.TransitionToBossFight);
        }

        #endregion

        public override void StartPhase()
        {
            GameObject boss = m_gameplayPlayerManager.Boss;
            Debug.Assert(boss != null);

            cameraHandleInstance = Instantiate(cameraHandlePrefab, boss.transform.position, Quaternion.Euler(0,90,0));
            cameraHandleInstance.GetComponent<MoveTowardsTarget>().target = cameraHandleTarget;

            Debug.Log("Boss should do something fancy");
            Debug.Log("Camera should continue to move forward");

            m_virtualCamera.Follow = cameraHandleInstance.transform;
            m_virtualCamera.LookAt = cameraHandleInstance.transform;
            m_virtualCamera.gameObject.SetActive(true);

            m_transitionStartTrigger.SetActive(false);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);

            m_phase2StartTrigger.SetActive(false);
            Destroy(cameraHandleInstance);
            cameraHandleInstance = null;
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            EndPhaseCommon();
        }
    }
}
