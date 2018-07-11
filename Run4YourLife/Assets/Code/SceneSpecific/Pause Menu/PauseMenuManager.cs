using UnityEngine;
using UnityEngine.EventSystems;

using Cinemachine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.SceneSpecific.PauseMenu
{
    public class PauseMenuManager : MonoBehaviour
    {
        public void OnContinueButtonClick()
        {
            PauseManager.Instance.ResumeGame();
        }
    }
}
