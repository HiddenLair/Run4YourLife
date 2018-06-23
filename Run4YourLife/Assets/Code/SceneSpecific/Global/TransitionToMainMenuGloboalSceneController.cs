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
        private GameObject uiSkipInfo;

        [SerializeField]
        private float uiSkipInfoTime0 = 1.0f;

        [SerializeField]
        private float uiSkipInfoTime1 = 3.5f;

        private void Start()
        {
            Debug.Assert(uiSkipInfoTime0 <= uiSkipInfoTime1 && uiSkipInfoTime0 >= 0.0f, "TransitionToMainMenuGloboalSceneController: Specified times are not correct!");

            StartCoroutine(UISkipInfoEnabler());
            StartCoroutine(YieldHelper.WaitForSeconds(()=> m_sceneTransitionRequest.Execute(), 17.0f));
        }

        private IEnumerator UISkipInfoEnabler()
        {
            uiSkipInfo.SetActive(false);

            yield return new WaitForSeconds(uiSkipInfoTime0);

            uiSkipInfo.SetActive(true);

            yield return new WaitForSeconds(uiSkipInfoTime1);

            uiSkipInfo.SetActive(false);
        }
    }
}