using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Run4YourLife.SceneManagement;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.GameManagement
{
    public enum PauseState
    {
        PAUSED,
        UNPAUSED
    }

    [RequireComponent(typeof(PlayersGameControlScheme))]
    public class PauseManager : SingletonMonoBehaviour<PauseManager>
    {
        [SerializeField]
        private SceneTransitionRequest m_pauseSceneLoader;

        [SerializeField]
        private SceneTransitionRequest m_pauseSceneUnloader;

        [SerializeField]
        private AudioClip m_pauseGameAudioClip;

        [SerializeField]
        private AudioClip m_resumeGameAudioClip;

        private PauseState m_pauseState = PauseState.UNPAUSED;

        private PlayersGameControlScheme m_playersGameControlScheme;

        private void Awake()
        {
            m_playersGameControlScheme = GetComponent<PlayersGameControlScheme>();
        }

        private void Update()
        {
            if (m_playersGameControlScheme.Pause.Started())
            {
                if (m_pauseState == PauseState.UNPAUSED)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }

        public void PauseGame()
        {
            Debug.Assert(m_pauseState == PauseState.UNPAUSED);
            m_pauseState = PauseState.PAUSED;
            Time.timeScale = 0;
            m_pauseSceneLoader.Execute();
            CameraManager.Instance.CinemachineBrain.enabled = false;
            AudioManager.Instance.PlaySFX(m_pauseGameAudioClip);
            AudioManager.Instance.PauseMusic();


            foreach (GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                runner.SetActive(false);
            }

            foreach (GameObject ghost in GameplayPlayerManager.Instance.GhostsAlive)
            {
                ghost.SetActive(false);
            }
        }

        public void ResumeGame()
        {
            Debug.Assert(m_pauseState == PauseState.PAUSED);
            m_pauseState = PauseState.UNPAUSED;
            Time.timeScale = 1;
            m_pauseSceneUnloader.Execute();
            CameraManager.Instance.CinemachineBrain.enabled = true;
            AudioManager.Instance.PlaySFX(m_resumeGameAudioClip);
            AudioManager.Instance.UnPauseMusic();


            foreach (GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                runner.SetActive(true);
            }

            foreach (GameObject ghost in GameplayPlayerManager.Instance.GhostsAlive)
            {
                ghost.SetActive(true);
            }
        }
    }
}