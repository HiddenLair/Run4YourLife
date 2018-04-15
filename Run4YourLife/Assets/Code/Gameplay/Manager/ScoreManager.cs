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

        Dictionary<PlayerDefinition, float> pointDictionary;

        #endregion

        private void Start()
        {
            List<PlayerDefinition> runnersDefinitions = FindObjectOfType<GameplayPlayerManager>().RunnerPlayerDefinitions;
            foreach(PlayerDefinition def in runnersDefinitions)
            {
                pointDictionary[def] = 0;
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
