using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public enum LoadEvent
    {
        Custom,
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
        private LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

        [SerializeField]
        private LoadEvent loadEvent = LoadEvent.Custom;

        [SerializeField]
        private bool setLoadedSceneAsActiveScene = true;

        [SerializeField]
        private bool unloadActiveScene = false;

        [SerializeField]
        private bool waitUntilUnloadCompletedToLoadNext = false;

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
            if(!unloadActiveScene && waitUntilUnloadCompletedToLoadNext)
            {
                Debug.LogError("If you don't unload the active scene, the unload event will not trigger and no scene will be loaded");
            }

            if(unloadActiveScene)
            {
                SceneManager.sceneUnloaded += SceneUnloaded;
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }

            if(!waitUntilUnloadCompletedToLoadNext)
            {
                SceneManager.sceneLoaded += SceneLoaded;
                SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            }
            
        }

        private void SceneUnloaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= SceneUnloaded;

            if (waitUntilUnloadCompletedToLoadNext)
            {
                SceneManager.sceneLoaded += SceneLoaded;
                SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            if (setLoadedSceneAsActiveScene)
            {
                SceneManager.SetActiveScene(scene);
            }
        }
    }
}