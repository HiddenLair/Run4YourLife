using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.UI;

namespace Run4YourLife.GameManagement
{
    public interface IScoreEvents : IEventSystemHandler, IGameplayEvents
    {
        void OnScoreAdded(PlayerHandle playerHandle, float points);
    }

    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>, IScoreEvents, IGameplayEvents
    {
        private Dictionary<PlayerHandle, float> m_score = new Dictionary<PlayerHandle, float>();
        private GameObject m_uiGameObject;

        private void Awake()
        {
            m_uiGameObject = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_uiGameObject != null);
        }

        public void OnScoreAdded(PlayerHandle playerHandle,float points)
        {
            if(!m_score.ContainsKey(playerHandle))
            {
                m_score.Add(playerHandle, 0);
            }

            float score = (m_score[playerHandle] += points);
            ExecuteEvents.Execute<IUIScoreEvents>(m_uiGameObject, null, (x,y)=>x.OnScoreChanged(playerHandle, score));
        }

        public void OnGameEnded(GameEndResult gameEndResult)
        {
            GlobalDataContainer.Instance.Data[GlobalDataContainerKeys.Score] = m_score;
        }
    }
}
