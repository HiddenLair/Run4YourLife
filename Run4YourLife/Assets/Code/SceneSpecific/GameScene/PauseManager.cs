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

    public class PauseManager : SingletonMonoBehaviour<PauseManager>, IPauseEvent
    {
        private PauseState actualGameState = PauseState.UNPAUSED;
        public SceneTransitionRequest m_pauseSceneLoader;
        public SceneTransitionRequest m_pauseSceneUnloader;
        public PauseStateChangeEvent PauseChangeEvent = new PauseStateChangeEvent();

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
    }
}