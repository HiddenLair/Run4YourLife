using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager>
    {
        private bool loadingScene = false;

        public bool IsSceneLoaded(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName).IsValid();
        }

        public AsyncOperation ExecuteRequest(SceneTransitionRequestData data)
        {
            if(data.loadScene && data.unloadScene)
            {
                Debug.LogError("LOAD + UNLOAD");
            }
            else if(data.loadScene && !loadingScene)
            {
                return LoadScene(data);
            }
            else if(data.unloadScene)
            {
                return UnloadScene(data);
            }
            else
            {
                Debug.LogError("Requesting a scene load, but did not specify a load nor unload");
            }

            return null;
        }
        
        private AsyncOperation UnloadScene(SceneTransitionRequestData data)
        {
            return SceneManager.UnloadSceneAsync(data.unloadedSceneName);
        }

        private AsyncOperation LoadScene(SceneTransitionRequestData data)
        {
            loadingScene = true;

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(data.sceneName, data.loadSceneMode);
            loadSceneAsync.allowSceneActivation = data.setLoadedSceneAsActiveScene;
            loadSceneAsync.completed += (op) => { loadingScene = false; };

            return loadSceneAsync;
        }
    }
}