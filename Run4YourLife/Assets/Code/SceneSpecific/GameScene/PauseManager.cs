using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;
using Run4YourLife.InputManagement;

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
        
        private PauseState m_pauseState = PauseState.UNPAUSED;
        
        private PlayersGameControlScheme m_playersGameControlScheme;

        private void Awake()
        {
            m_playersGameControlScheme = GetComponent<PlayersGameControlScheme>();
        }

        private void Update()
        {
            if(m_playersGameControlScheme.Pause.Started())
            {
                if(m_pauseState == PauseState.UNPAUSED)
                {
                    PauseGame();
                }
                else
                {
                    UnPauseGame();
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

            foreach(GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                runner.SetActive(false);
            }

            foreach(GameObject ghost in GameplayPlayerManager.Instance.GhostsAlive)
            {
                ghost.SetActive(false);
            }
        }

        public void UnPauseGame()
        {
            Debug.Assert(m_pauseState == PauseState.PAUSED);
            m_pauseState = PauseState.UNPAUSED;
            Time.timeScale = 1;
            m_pauseSceneUnloader.Execute();
            CameraManager.Instance.CinemachineBrain.enabled = true;

            foreach(GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                runner.SetActive(true);
            }

            foreach(GameObject ghost in GameplayPlayerManager.Instance.GhostsAlive)
            {
                ghost.SetActive(true);
            }
        }
    }
}