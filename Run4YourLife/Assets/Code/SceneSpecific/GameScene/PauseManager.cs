using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.GameManagement
{
    public enum PauseState
    {
        PAUSED,
        UNPAUSED
    }

    public interface IPauseEvent : IEventSystemHandler
    {
        void OnPauseInput();
        void AttachListener(UnityAction<PauseState> pauseListenerAction);
        void DetachListener(UnityAction<PauseState> pauseListenerAction);
    }

    public class PauseStateChangeEvent : UnityEvent<PauseState> { }

    public class PauseManager : SingletonMonoBehaviour<PauseManager>, IPauseEvent
    {
        private PauseState actualGameState = PauseState.UNPAUSED;
        public SceneTransitionRequest m_pauseSceneLoader;
        public SceneTransitionRequest m_pauseSceneUnloader;
        private PauseStateChangeEvent PauseChangeEvent = new PauseStateChangeEvent();

        public void OnPauseInput()
        {
            switch(actualGameState)
            {
                case PauseState.PAUSED:
                    {
                        CameraManager.Instance.CinemachineBrain.enabled = true;
                        
                        Time.timeScale = 1;
                        actualGameState = PauseState.UNPAUSED;
                        m_pauseSceneUnloader.Execute();
                    }
                    break;
                case PauseState.UNPAUSED:
                    {
                        CameraManager.Instance.CinemachineBrain.enabled = false;

                        Time.timeScale = 0;
                        actualGameState = PauseState.PAUSED;
                        m_pauseSceneLoader.Execute();
                    }
                    break;
            }

            PauseChangeEvent.Invoke(actualGameState);
        }

        public void AttachListener(UnityAction<PauseState> pauseListenerAction)
        {
            PauseChangeEvent.AddListener(pauseListenerAction);
        }

        public void DetachListener(UnityAction<PauseState> pauseListenerAction)
        {
            PauseChangeEvent.RemoveListener(pauseListenerAction);
        }
    }
}