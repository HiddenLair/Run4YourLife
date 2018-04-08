using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Run4YourLife.SceneManagement;

namespace Run4YourLife.OptionsMenu
{
    public class OptionsMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneLoadRequest m_mainMenuLoadRequest;

        public void OnExitButtonPressed()
        {
            m_mainMenuLoadRequest.Execute();
        }
    }
}
