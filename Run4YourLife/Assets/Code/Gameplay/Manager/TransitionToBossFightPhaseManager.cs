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
        private GameObject m_phase1to2Bridge;

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

        #region Initialization

        private void Awake()
        {
            RegisterPhase(GamePhase.TransitionToBossFight);
        }

        #endregion

        public override void StartPhase()
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            Debug.Assert(boss != null);

            cameraHandleInstance = Instantiate(cameraHandlePrefab, boss.transform.position, Quaternion.Euler(0,90,0));
            cameraHandleInstance.GetComponent<MoveTowardsTarget>().target = cameraHandleTarget;

            Debug.Log("Boss should do something fancy");
            Debug.Log("Camera should continue to move forward");
            Destroy(boss);

            m_virtualCamera.Follow = cameraHandleInstance.transform;
            m_virtualCamera.LookAt = cameraHandleInstance.transform;
            m_virtualCamera.gameObject.SetActive(true);

            m_transitionStartTrigger.SetActive(false);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            StartCoroutine(EndPhaseCoroutine());
        }

        private IEnumerator EndPhaseCoroutine()
        {
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);

            m_phase2StartTrigger.SetActive(false);
            Destroy(cameraHandleInstance);
            cameraHandleInstance = null;
            yield return new WaitForSeconds(4);
            m_phase1to2Bridge.SetActive(false);
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            StopAllCoroutines();
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);
            m_phase2StartTrigger.SetActive(false);
            Destroy(cameraHandleInstance);
            cameraHandleInstance = null;
            m_phase2StartTrigger.SetActive(false);
            m_phase1to2Bridge.SetActive(false);
        }
    }
}
