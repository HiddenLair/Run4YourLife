using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager>
    {
        private bool loadingScene = false;

        public void ExecuteRequest(SceneTransitionRequestData data)
        {
            if(data.loadScene && data.unloadScene)
            {
                if(!loadingScene)
                {
                    StartCoroutine(LoadUnloadScene(data));
                }
            }
            else if(data.loadScene)
            {
                if(!loadingScene)
                {
                    LoadScene(data);
                }
            }
            else if(data.unloadScene)
            {
                UnloadScene(data);
            }
            else
            {
                Debug.LogError("Requesting a scene load, but did not specify a load nor unload");
            }
        }
        
        private AsyncOperation UnloadScene(SceneTransitionRequestData data)
        {
            return SceneManager.UnloadSceneAsync(data.unloadedSceneName);
        }

        private AsyncOperation LoadScene(SceneTransitionRequestData data)
        {
            loadingScene = true;

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(data.sceneName, data.loadSceneMode);
            loadSceneAsync.completed += (op) => { loadingScene = false; };

            return loadSceneAsync;
        }

        private IEnumerator LoadUnloadScene(SceneTransitionRequestData data)
        {
            AsyncOperation unloadSceneAsync = UnloadScene(data);

            AsyncOperation loadSceneAsync = LoadScene(data);
            loadSceneAsync.allowSceneActivation = false;

            while(!loadSceneAsync.isDone)
            {
                // loading bar progress
                //_loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

                // scene has loaded as much as possible, the last 10% can't be multi-threaded
                if(loadSceneAsync.progress >= 0.9f)
                {
                    yield return new WaitUntil(() => unloadSceneAsync.isDone);
                    loadSceneAsync.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}