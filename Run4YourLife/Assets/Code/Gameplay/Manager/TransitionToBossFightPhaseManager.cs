using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using System;

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
        private CameraTargetCentered m_cameraTargetCentered;

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

            m_cameraTargetCentered.m_target = cameraHandleInstance.transform;
            m_cameraTargetCentered.enabled = true;

            m_transitionStartTrigger.SetActive(false);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            StartCoroutine(EndPhaseCoroutine());
        }

        private IEnumerator EndPhaseCoroutine()
        {
            m_cameraTargetCentered.enabled = false;
            m_cameraTargetCentered.m_target = null;
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
            m_cameraTargetCentered.m_target = null;
            m_phase2StartTrigger.SetActive(false);
            Destroy(cameraHandleInstance);
            cameraHandleInstance = null;
            m_phase2StartTrigger.SetActive(false);
            m_phase1to2Bridge.SetActive(false);
        }
    }
}
