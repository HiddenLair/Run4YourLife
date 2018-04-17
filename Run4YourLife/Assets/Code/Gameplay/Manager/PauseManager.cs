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
        public SceneLoadRequest m_pauseSceneLoader;

        public void OnPauseInput()
        {
            Time.timeScale = 0;
            m_pauseSceneLoader.Execute();
        }
    }
}