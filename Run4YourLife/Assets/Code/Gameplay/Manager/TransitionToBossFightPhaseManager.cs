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
        #endregion

        #region Initialization

        private void Awake()
        {
            RegisterPhase(GamePhase.TransitionToBossFight);
        }

        #endregion

        public override void StartPhase()
        {
            Debug.Log("Boss should do something fancy");
            Debug.Log("Camera should continue to move forward");
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            Debug.Assert(boss != null);
            Destroy(boss);

            m_transitionStartTrigger.SetActive(false);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            StartCoroutine(EndPhaseCoroutine());
        }

        private IEnumerator EndPhaseCoroutine()
        {
            m_phase2StartTrigger.SetActive(false);
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
            m_phase2StartTrigger.SetActive(false);
            m_phase1to2Bridge.SetActive(false);
        }
    }
}
