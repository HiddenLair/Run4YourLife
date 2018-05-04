using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.GameManagement
{
    public enum GamePhase
    {
        TransitionToEasyMoveHorizontal,
        EasyMoveHorizontal,
        TransitionToBossFight,
        BossFight,
        BossFightRockTransition,
        TransitionToHardMoveHorizontal,
        HardMoveHorizontal,
        End
    }

    [System.Serializable]
    public class GamePhaseEvent : UnityEvent<GamePhase>
    {
    }

    public abstract class GamePhaseManager : MonoBehaviour {
        public abstract GamePhase GamePhase { get; }

        public abstract void StartPhase();
        public abstract void EndPhase();

        public abstract void DebugStartPhase();
        public abstract void DebugEndPhase();
    }
}

