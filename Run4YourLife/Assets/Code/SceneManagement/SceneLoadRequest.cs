using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public enum LoadEvent
    {
        Custom,
        Start,
        OnDisable,
        OnDestroy
    }

    [System.Serializable]
    public class SceneLoadRequestData
    {
        public bool loadScene = true;
        public string sceneName;
        public LoadSceneMode loadSceneMode = LoadSceneMode.Additive;
        public LoadEvent loadEvent = LoadEvent.Custom;
        public bool setLoadedSceneAsActiveScene = true;
        public bool unloadScene = false;
        public string unloadedSceneName;
    }

    public class SceneLoadRequest : MonoBehaviour
    {
        public SceneLoadRequestData sceneLoadRequestData;

        private SceneLoader m_sceneLoader;

        private void Awake()
        {
            m_sceneLoader = GetOrCreateDefaultSceneLoader();
            Debug.Assert(m_sceneLoader != null);
        }

        private SceneLoader GetOrCreateDefaultSceneLoader()
        {
            SceneLoader ret = FindObjectOfType<SceneLoader>();
            if(ret == null)
            {
                ret = gameObject.AddComponent<SceneLoader>();
            }

            return ret;
        }

        private void Start()
        {
            if (sceneLoadRequestData.loadEvent.Equals(LoadEvent.Start))
            {
                Execute();
            }
        }

        private void OnDisable()
        {
            if (sceneLoadRequestData.loadEvent.Equals(LoadEvent.OnDisable))
            {
                Execute();
            }
        }

        private void OnDestroy()
        {
            if (sceneLoadRequestData.loadEvent.Equals(LoadEvent.OnDestroy))
            {
                Execute();
            }
        }

        public void Execute()
        {
            m_sceneLoader.ExecuteRequest(sceneLoadRequestData);
        }

    }
}