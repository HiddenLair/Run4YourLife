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
        private SceneLoadRequest m_characterSelectionLoadRequest;

        public void OnPlayButtonPressed()
        {
            m_characterSelectionLoadRequest.Execute();
        }

        public void OnOptionsButtonPressed()
        {
            Debug.Log("OnOptionsButtonPressed");
        }

        public void OnExitButtonPressed()
        {
            Debug.Log("OnExitButtonPressed");
        }
    }
}