using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.GameManagement
{
    public enum GamePhase
    {
        TransitionToEasyMoveHorizontal,
        EasyMoveHorizontal,
        TransitionToBossFight,
        BossFight,
        TransitionToHardMoveHorizontal,
        HardMoveHorizontal,
        End
    }

    public abstract class GamePhaseManager : MonoBehaviour {
        public GamePhase GamePhase { get; private set; }

        public void RegisterPhase(GamePhase gamePhase)
        {
            GamePhase = gamePhase;
            FindObjectOfType<GameManager>().RegisterPhase(this.GamePhase,this);
        }

        public abstract void StartPhase();
        public abstract void EndPhase();

        public abstract void DebugStartPhase();
        public abstract void DebugEndPhase();
    }
}

