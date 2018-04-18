using Cinemachine;
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

        private CinemachineBrain mainCameraCinemachine;

        private void Awake()
        {
            mainCameraCinemachine = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void OnDestroy()
        {
            if (mainCameraCinemachine != null)
            {
                mainCameraCinemachine.enabled = true;
            }

            Time.timeScale = 1;
        }
    }
}
