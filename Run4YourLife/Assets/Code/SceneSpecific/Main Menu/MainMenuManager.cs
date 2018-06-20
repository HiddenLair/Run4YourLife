using UnityEngine;

using Run4YourLife.SceneManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.SceneSpecific.CharacterSelection;

namespace Run4YourLife.SceneSpecific.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_mainMenuMusicClip;

        [SerializeField]
        private SceneTransitionRequest m_characterSelectionLoadRequest;

        [SerializeField]
        private SceneTransitionRequest m_optionsMenuLoadRequest;

        private void Start()
        {
            AudioManager.Instance.PlayMusic(m_mainMenuMusicClip);
        }

        public void OnPlayButtonPressed()
        {
            GlobalDataContainer.Instance.Set(CharacterSelectionManager.CharacterSelectionTargetKey, CharacterSelectionManager.CharacterSelectionTarget.Game);
            m_characterSelectionLoadRequest.Execute();
        }

        public void OnTutorialButtonPressed()
        {
            GlobalDataContainer.Instance.Set(CharacterSelectionManager.CharacterSelectionTargetKey, CharacterSelectionManager.CharacterSelectionTarget.Tutorial);
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