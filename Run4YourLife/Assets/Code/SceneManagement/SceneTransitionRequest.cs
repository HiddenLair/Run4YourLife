using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    [Serializable]
    public class SceneTransitionRequestData
    {
        public bool loadScene;
        public string sceneName;
        public LoadSceneMode loadSceneMode;
        public bool setLoadedSceneAsActiveScene;
        public bool unloadScene;
        public string unloadedSceneName;
    }

    public class SceneTransitionRequest : MonoBehaviour
    {
        public enum ExecutionEvent
        {
            Custom,
            Start
        }

        [SerializeField]
        private ExecutionEvent m_loadEvent = ExecutionEvent.Custom;

        [SerializeField]
        private SceneTransitionRequestData m_sceneLoadRequestData;

        private AsyncOperation request;

        private void Start()
        {
            if(m_loadEvent.Equals(ExecutionEvent.Start))
            {
                Execute();
            }
        }

        public AsyncOperation GetRequest()
        {
            return request;
        }

        public void Execute()
        {
            request = SceneTransitionManager.Instance.ExecuteRequest(m_sceneLoadRequestData);
        }
    }
}