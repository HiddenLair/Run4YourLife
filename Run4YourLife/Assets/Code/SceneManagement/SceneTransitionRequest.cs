using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    [System.Serializable]
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
        private SceneTransitionRequest.ExecutionEvent m_loadEvent = SceneTransitionRequest.ExecutionEvent.Custom;

        [SerializeField]
        private SceneTransitionRequestData m_sceneLoadRequestData;

        private void Start()
        {
            if (m_loadEvent.Equals(ExecutionEvent.Start))
            {
                Execute();
            }
        }

        public void Execute()
        {
            SceneTransitionManager.Instance.ExecuteRequest(m_sceneLoadRequestData);
        }

    }
}