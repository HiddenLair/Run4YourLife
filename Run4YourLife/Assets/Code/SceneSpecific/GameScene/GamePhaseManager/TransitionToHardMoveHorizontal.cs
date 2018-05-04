using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToHardMoveHorizontal : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionToHardMoveHorizontal;  } }

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
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            EndPhaseCommon();
        }

        #endregion

    }
}

