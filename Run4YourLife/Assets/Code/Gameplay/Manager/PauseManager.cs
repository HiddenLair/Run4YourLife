using Cinemachine;
using Run4YourLife.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    }

    public class PauseStateChangeEvent : UnityEvent<PauseState> { }

    public class PauseManager : MonoBehaviour, IPauseEvent
    {
        private PauseState actualGameState = PauseState.UNPAUSED;
        private CinemachineBrain mainCameraCinemachine;
        public SceneLoadRequest m_pauseSceneLoader;
        public SceneLoadRequest m_pauseSceneUnloader;
        public PauseStateChangeEvent PauseChangeEvent = new PauseStateChangeEvent();

        private void Awake()
        {
            mainCameraCinemachine = Camera.main.GetComponent<CinemachineBrain>();
        }

        public void OnPauseInput()
        {
            switch(actualGameState)
            {
                case PauseState.PAUSED:
                    {
                        if (mainCameraCinemachine != null)
                        {
                            mainCameraCinemachine.enabled = true;
                        }
                        Time.timeScale = 1;
                        actualGameState = PauseState.UNPAUSED;
                        m_pauseSceneUnloader.Execute();
                    }
                    break;
                case PauseState.UNPAUSED:
                    {
                        if (mainCameraCinemachine != null)
                        {
                            mainCameraCinemachine.enabled = false;
                        }
                        Time.timeScale = 0;
                        actualGameState = PauseState.PAUSED;
                        m_pauseSceneLoader.Execute();
                    }
                    break;
            }

            PauseChangeEvent.Invoke(actualGameState);
        }
    }
}