using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public enum LoadEvent
    {
        Custom,
        Awake,
        Start,
        OnEnable,
        OnDisable,
        OnDestroy
    }

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        [SerializeField]
        private LoadSceneMode loadSceneMode;

        [SerializeField]
        private LoadEvent loadEvent;

        void Awake()
        {
            if (loadEvent.Equals(LoadEvent.Awake))
            {
                LoadScene();
            }
        }

        void Start()
        {
            if (loadEvent.Equals(LoadEvent.Start))
            {
                LoadScene();
            }
        }

        void OnEnable()
        {
            if (loadEvent.Equals(LoadEvent.OnEnable))
            {
                LoadScene();
            }
        }

        void OnDisable()
        {
            if (loadEvent.Equals(LoadEvent.OnDisable))
            {
                LoadScene();
            }
        }

        void OnDestroy()
        {
            if (loadEvent.Equals(LoadEvent.OnDestroy))
            {
                LoadScene();
            }
        }

        public void LoadScene()
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }
    }
}