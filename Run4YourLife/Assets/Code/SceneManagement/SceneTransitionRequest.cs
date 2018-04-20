using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    [System.Serializable]
    public class SceneTransitionRequestData
    {
        public bool loadScene = true;
        public string sceneName;
        public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        public SceneTransitionRequest.ExecutionEvent loadEvent = SceneTransitionRequest.ExecutionEvent.Custom;
        public bool setLoadedSceneAsActiveScene = true;
        public bool unloadScene = false;
        public string unloadedSceneName;
    }

    public class SceneTransitionRequest : MonoBehaviour
    {
        public enum ExecutionEvent
        {
            Custom,
            Start
        }

        public SceneTransitionRequestData sceneLoadRequestData;

        private SceneTransitionManager m_sceneLoader;

        private void Awake()
        {
            m_sceneLoader = GetOrCreateDefaultSceneLoader();
            Debug.Assert(m_sceneLoader != null);
        }

        private SceneTransitionManager GetOrCreateDefaultSceneLoader()
        {
            SceneTransitionManager ret = FindObjectOfType<SceneTransitionManager>();
            if(ret == null)
            {
                ret = gameObject.AddComponent<SceneTransitionManager>();
            }

            return ret;
        }

        private void Start()
        {
            if (sceneLoadRequestData.loadEvent.Equals(ExecutionEvent.Start))
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