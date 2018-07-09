using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.GameManagement
{
    public enum GamePhase
    {
        TransitionPhase1Start,
        EasyMoveHorizontal,
        TransitionPhase1End,
        TransitionPhase2Start,
        BossFight,
        TransitionPhase2End,
        TransitionPhase3Start,
        HardMoveHorizontal,
        End,
        StartTutorial,
        BossTutorial,
        RunnersWin,
        BossWin
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

