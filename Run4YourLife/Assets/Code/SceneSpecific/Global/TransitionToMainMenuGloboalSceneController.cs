using UnityEngine;
using System.Collections;
using Run4YourLife.SceneManagement;

using Run4YourLife.Utils;

namespace Run4YourLife.SceneSpecific.Global
{
    public class TransitionToMainMenuGloboalSceneController : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest m_sceneTransitionRequest;

        [SerializeField]
        private CanvasGroup uiSkipInfo;

        [SerializeField]
        private float uiSkipInfoTime0 = 1.0f;

        [SerializeField]
        private float uiSkipInfoTime1 = 3.5f;

        [SerializeField]
        private float uiSkipInfoFadeTime = 0.5f;

        private void Start()
        {
            Debug.Assert(uiSkipInfoTime0 <= uiSkipInfoTime1 && uiSkipInfoTime0 >= 0.0f, "TransitionToMainMenuGloboalSceneController: Specified times are not correct!");

            StartCoroutine(UISkipInfoUpdater());
            StartCoroutine(YieldHelper.WaitForSeconds(() => m_sceneTransitionRequest.Execute(), 16.25f));
        }

        private IEnumerator UISkipInfoUpdater()
        {
            uiSkipInfo.alpha = 0.0f;

            yield return new WaitForSeconds(uiSkipInfoTime0);

            StartCoroutine(UIAlphaUpdater(0.0f, 1.0f, uiSkipInfoFadeTime));

            yield return new WaitForSeconds(uiSkipInfoTime1);

            StartCoroutine(UIAlphaUpdater(1.0f, 0.0f, uiSkipInfoFadeTime));
        }

        private IEnumerator UIAlphaUpdater(float alpha0, float alpha1, float timeS)
        {
            float iTime = Time.time;
            float cTime = iTime;
            float eTime = iTime + timeS;

            while(cTime < eTime)
            {
                uiSkipInfo.alpha = Mathf.Clamp01(Mathf.Lerp(alpha0, alpha1, (cTime - iTime) / timeS));

                yield return null;
                cTime = Time.time;
            }

            uiSkipInfo.alpha = alpha1;
        }
    }
}