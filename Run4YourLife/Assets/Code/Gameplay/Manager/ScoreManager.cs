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
        void OnAddPoints(PlayerHandle playerDefinition,float points);
    }

    public class ScoreManager : MonoBehaviour,IScoreEvents
    {

        #region Variables

        Dictionary<PlayerHandle, float> pointDictionary = new Dictionary<PlayerHandle, float>();

        #endregion

        public void Initialize()
        {
            pointDictionary.Clear();

            foreach(PlayerHandle playerDefinition in FindObjectOfType<PlayerManager>().GetRunners())
            {
                pointDictionary[playerDefinition] = 0;
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
            pointDictionary[playerDefinition] += points;
        }

        public float GetPointsByPlayerDefinition(PlayerHandle playerDefinition)
        {
            return pointDictionary[playerDefinition];
        }
    }
}
