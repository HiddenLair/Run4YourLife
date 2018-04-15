using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public interface IScoreEvents : IEventSystemHandler
    {
        void OnAddPoints(PlayerDefinition playerDefinition,float points);
    }

    public class ScoreManager : MonoBehaviour,IScoreEvents
    {

        #region Variables

        Dictionary<PlayerDefinition, float> pointDictionary = new Dictionary<PlayerDefinition, float>();

        #endregion

        void Start()
        {
            foreach(PlayerDefinition playerDefinition in FindObjectOfType<PlayerManager>().GetRunners())
            {
                pointDictionary[playerDefinition] = 0;
            }
        }

        public void OnAddPoints(PlayerDefinition playerDefinition,float points)
        {
            pointDictionary[playerDefinition] += points;
        }

        public float GetPointsByPlayerDefinition(PlayerDefinition playerDefinition)
        {
            return pointDictionary[playerDefinition];
        }
    }
}
