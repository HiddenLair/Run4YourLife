using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public void OnPlayButtonPressed()
        {
            Scene mainMenuScene = gameObject.scene;
            SceneManager.UnloadSceneAsync(mainMenuScene);
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