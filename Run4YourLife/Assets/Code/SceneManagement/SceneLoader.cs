using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public void ExecuteRequest(SceneLoadRequestData data)
        {
            if (data.loadScene && data.unloadScene)
            {
                StartCoroutine(LoadUnloadScene(data));
            }
            else if (data.loadScene)
            {
                LoadScene(data);
            }
            else if (data.unloadScene)
            {
                UnloadScene(data);
            }
            else
            {
                Debug.LogError("Requesting a scene load, but did not specify a load nor unload");
            }
        }
        
        private void UnloadScene(SceneLoadRequestData data)
        {
            SceneManager.UnloadSceneAsync(data.unloadedSceneName);
        }

        private void LoadScene(SceneLoadRequestData data)
        {
            SceneManager.LoadSceneAsync(data.sceneName, data.loadSceneMode);
        }

        private IEnumerator LoadUnloadScene(SceneLoadRequestData data)
        {
            AsyncOperation unloadSceneAsync = null;

            if (data.unloadScene)
            {
                unloadSceneAsync = SceneManager.UnloadSceneAsync(data.unloadedSceneName);
            }

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(data.sceneName, data.loadSceneMode);
            loadSceneAsync.allowSceneActivation = false;

            while (!loadSceneAsync.isDone)
            {
                // loading bar progress
                //_loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

                // scene has loaded as much as possible, the last 10% can't be multi-threaded
                if (loadSceneAsync.progress >= 0.9f)
                {
                    if (unloadSceneAsync != null)
                    {
                        yield return new WaitUntil(() => unloadSceneAsync.isDone);
                    }
                    loadSceneAsync.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}