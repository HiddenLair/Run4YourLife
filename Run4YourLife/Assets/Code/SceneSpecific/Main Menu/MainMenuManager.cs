using UnityEngine;

using Run4YourLife.SceneManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest m_characterSelectionLoadRequest;

        [SerializeField]
        private SceneTransitionRequest m_optionsMenuLoadRequest;

        private void Start()
        {
            AudioManager.Instance.PlayMusic(AudioManager.Music.MainMenu);
        }

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