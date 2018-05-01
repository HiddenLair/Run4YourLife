using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToHardMoveHorizontal : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

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
            GameManager.Instance.EndExecutingPhaseAndStartPhase(GamePhase.HardMoveHorizontal);
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
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            UnityEngine.Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            EndPhaseCommon();
        }

        #endregion

    }
}

