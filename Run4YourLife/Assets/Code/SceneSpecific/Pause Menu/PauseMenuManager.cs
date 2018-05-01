using UnityEngine;
using UnityEngine.EventSystems;

using Cinemachine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.SceneSpecific.PauseMenu
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
