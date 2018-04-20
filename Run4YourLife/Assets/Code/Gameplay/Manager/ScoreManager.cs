using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;
using UnityEngine.SceneManagement;

namespace Run4YourLife.GameManagement
{
    public interface IScoreEvents : IEventSystemHandler
    {
        void OnAddPoints(PlayerHandle playerDefinition, float points);
    }

    [System.Serializable]
    public class ScoreChangeEvent : UnityEvent<PlayerHandle, float> { }

    public class ScoreManager : MonoBehaviour,IScoreEvents
    {
        private ScoreChangeEvent m_onPlayerScoreChanged = new ScoreChangeEvent();
        public ScoreChangeEvent OnPlayerScoreChanged { get { return m_onPlayerScoreChanged; } }

        private Dictionary<PlayerHandle, float> m_playerScore = new Dictionary<PlayerHandle, float>();

        public void Initialize()
        {
            m_playerScore.Clear();

            foreach(PlayerHandle playerDefinition in FindObjectOfType<PlayerManager>().GetRunners())
            {
                m_playerScore[playerDefinition] = 0;
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "GameScene")
            {
                Initialize();
            }
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnAddPoints(PlayerHandle playerDefinition,float points)
        {
            float score = (m_playerScore[playerDefinition] += points);
            m_onPlayerScoreChanged.Invoke(playerDefinition, score);
        }

        public float GetPointsByPlayerDefinition(PlayerHandle playerDefinition)
        {
            return m_playerScore[playerDefinition];
        }
    }
}
