using Cinemachine;
using Run4YourLife.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.GameManagement
{
    public interface IPauseEvent : IEventSystemHandler
    {
        void OnPauseInput();
    }

    public class PauseManager : MonoBehaviour, IPauseEvent
    {
        private CinemachineBrain mainCameraCinemachine;
        public SceneLoadRequest m_pauseSceneLoader;

        private void Awake()
        {
            mainCameraCinemachine = GetComponent<CinemachineBrain>();
        }

        public void OnPauseInput()
        {
            if (mainCameraCinemachine != null)
            {
                mainCameraCinemachine.enabled = false;
            }

            Time.timeScale = 0;
            m_pauseSceneLoader.Execute();
        }
    }
}