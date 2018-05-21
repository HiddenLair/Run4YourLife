using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.UI;

namespace Run4YourLife.GameManagement
{
    public interface IScoreEvents : IEventSystemHandler
    {
        void OnScoreAdded(PlayerHandle playerHandle, int score);
    }

    public interface IRunnerScoreEvents : IEventSystemHandler
    {
        void OnScoreChanged(int score);
    }

    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>, IScoreEvents, IGameplayEvents
    {
        private Dictionary<PlayerHandle, int> m_score = new Dictionary<PlayerHandle, int>();
        private GameObject m_uiGameObject;

        private void Awake()
        {
            m_uiGameObject = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_uiGameObject != null);
        }

        public void OnScoreAdded(PlayerHandle playerHandle, int score)
        {
            if(!m_score.ContainsKey(playerHandle))
            {
                m_score.Add(playerHandle, 0);
            }

            int newScore = (m_score[playerHandle] += score);

            GameObject runner = GameplayPlayerManager.Instance.RunnerGameObject[playerHandle];
            runner.GetComponent<IRunnerScoreEvents>().OnScoreChanged(newScore);
        }

        public void OnGameEnded(GameEndResult gameEndResult)
        {
            GlobalDataContainer.Instance.Data[GlobalDataContainerKeys.Score] = m_score;
        }
    }
}
