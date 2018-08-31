using TMPro;
using UnityEngine;
using System.Collections;

using Run4YourLife.SceneManagement;

namespace Run4YourLife.SceneSpecific.GameLoadingMenu
{
    public class GameLoadingMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest gameSceneLoadRequest;

        [SerializeField]
        private TextMeshProUGUI loadingText;

        void Start()
        {
            StartCoroutine(UpdateProgress());
        }

        private IEnumerator UpdateProgress()
        {
            yield return new WaitUntil(() => gameSceneLoadRequest.GetRequest() != null);

            while(gameSceneLoadRequest.GetRequest().progress < 0.9f)
            {
                OnLoading();
                yield return null;
            }

            OnCompleted();

            while(true)
            {
                if(Input.anyKeyDown)
                {
                    gameSceneLoadRequest.GetRequest().allowSceneActivation = true;
                    break;
                }

                yield return null;
            }
        }

        private void OnLoading()
        {
            loadingText.text = "Loading... " + GetProgress() + "%";
        }

        private void OnCompleted()
        {
            loadingText.color = Color.green;
            loadingText.text = "Done! Press Any Button";
        }

        private int GetProgress()
        {
            return (int)(100.0f * Mathf.Clamp01(gameSceneLoadRequest.GetRequest().progress / 0.9f));
        }
    }
}