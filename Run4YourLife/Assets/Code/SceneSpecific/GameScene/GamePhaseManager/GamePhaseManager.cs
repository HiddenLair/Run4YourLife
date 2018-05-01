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
        public GamePhase GamePhase { get; private set; }

        public void RegisterPhase(GamePhase gamePhase)
        {
            GamePhase = gamePhase;
            GameManager.Instance.RegisterPhase(this.GamePhase,this);
        }

        public abstract void StartPhase();
        public abstract void EndPhase();

        public abstract void DebugStartPhase();
        public abstract void DebugEndPhase();
    }
}

