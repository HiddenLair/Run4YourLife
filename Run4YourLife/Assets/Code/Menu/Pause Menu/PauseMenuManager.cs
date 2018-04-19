using Cinemachine;
using Run4YourLife.GameManagement;
using Run4YourLife.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Run4YourLife.PauseMenu
{
    public class PauseMenuManager : MonoBehaviour
    {
        private CinemachineBrain mainCameraCinemachine;
        private GameObject gameManager;

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag(Tags.GameController);
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

        public void OnContinueButtonClick()
        {
            ExecuteEvents.Execute<IPauseEvent>(gameManager, null, (x, y) => x.OnPauseInput());
        }
    }
}
