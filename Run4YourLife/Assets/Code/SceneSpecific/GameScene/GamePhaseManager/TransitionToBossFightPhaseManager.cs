using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToBossFightPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionToBossFight; } }

        public override void StartPhase()
        {                     
            GameManager.Instance.EndExecutingPhaseAndStartPhase(GamePhase.BossFight);
        }

        public override void EndPhase()
        {
            
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            
        }
    }
}
