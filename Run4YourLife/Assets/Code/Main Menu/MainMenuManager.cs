using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public void OnPlayButtonPressed()
        {
            Debug.Log("OnPlayButtonPressed");
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