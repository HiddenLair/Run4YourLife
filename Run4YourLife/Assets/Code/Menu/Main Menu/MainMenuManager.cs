using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Run4YourLife.SceneManagement;

namespace Run4YourLife.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest m_characterSelectionLoadRequest;

        [SerializeField]
        private SceneTransitionRequest m_optionsMenuLoadRequest;

        public void OnPlayButtonPressed()
        {
            m_characterSelectionLoadRequest.Execute();
        }

        public void OnOptionsButtonPressed()
        {
            m_optionsMenuLoadRequest.Execute();
        }

        public void OnExitButtonPressed()
        {
            Application.Quit();
        }
    }
}