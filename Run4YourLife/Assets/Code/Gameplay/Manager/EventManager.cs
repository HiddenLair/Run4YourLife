using System;
using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.GameManagement
{
    public enum PauseState
    {
        PAUSED,
        UNPAUSED
    }

    public class EventManager : MonoBehaviour
    {
        public UnityEvent<PauseState> onGamePause;
    }
}
