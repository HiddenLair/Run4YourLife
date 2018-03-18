using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToHardMoveHorizontal : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private GameObject m_transitionToHardMoveHorizontalBrige;

        [SerializeField]
        private CameraTargetCentered m_cameraTargetCentered;

        [SerializeField]
        private Transform m_cameraTargetHandle;

        #endregion

        #region Initialization

        private void Awake()
        {
            RegisterPhase(GamePhase.TransitionToHardMoveHorizontal);
        }

        #endregion

        #region Regular Execution


        public override void StartPhase()
        {
            m_transitionToHardMoveHorizontalBrige.SetActive(true);

            m_cameraTargetCentered.m_target = m_cameraTargetHandle;
            m_cameraTargetCentered.enabled = true;
        }

        public override void EndPhase()
        {
            m_cameraTargetCentered.m_target = null;
            m_cameraTargetCentered.enabled = false;
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
        }

        public override void DebugEndPhase()
        {
            m_cameraTargetCentered.m_target = null;
            m_cameraTargetCentered.enabled = false;
        }

        #endregion

    }
}

