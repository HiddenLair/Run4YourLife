using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;
using UnityEngine.SceneManagement;
using Run4YourLife.UI;

namespace Run4YourLife.GameManagement
{
    public interface IScoreEvents : IEventSystemHandler
    {
        void OnScoreAdded(PlayerHandle playerHandle, float points);
    }

    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>, IScoreEvents
    {
        private Dictionary<PlayerHandle, float> m_playerScore = new Dictionary<PlayerHandle, float>();
        private GameObject m_uiGameObject;

        private void Awake()
        {
            m_uiGameObject = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_uiGameObject != null);
        }

        public void OnScoreAdded(PlayerHandle playerHandle,float points)
        {
            if(!m_playerScore.ContainsKey(playerHandle))
            {
                m_playerScore.Add(playerHandle, 0);
            }

            float score = (m_playerScore[playerHandle] += points);
            ExecuteEvents.Execute<IUIScoreEvents>(m_uiGameObject, null, (x,y)=>x.OnScoreChanged(playerHandle, score));
        }
    }
}
