using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run4YourLife.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public void ExecuteRequest(SceneLoadRequestData sceneLoadRequestData)
        {
            StartCoroutine(CoroutineExecuteRequest(sceneLoadRequestData));
        }

        private IEnumerator CoroutineExecuteRequest(SceneLoadRequestData data)
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
                    if(unloadSceneAsync != null)
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