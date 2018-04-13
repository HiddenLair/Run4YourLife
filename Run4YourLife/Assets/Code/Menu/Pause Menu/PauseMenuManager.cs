using Run4YourLife.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.PauseMenu
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneLoadRequest m_mainMenuLoadRequest;

        [SerializeField]
        private SceneLoadRequest m_gameSceneLoadRequest;

        [SerializeField]
        private SceneLoadRequest m_optionsMenuLoadRequest;

        public void OnExitButtonPressed()
        {
            m_mainMenuLoadRequest.Execute();
        }
    }
}
