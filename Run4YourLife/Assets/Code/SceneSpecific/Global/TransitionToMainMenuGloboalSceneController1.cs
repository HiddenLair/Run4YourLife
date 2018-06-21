using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Run4YourLife.SceneManagement;

using Run4YourLife.Utils;

namespace Run4YourLife.SceneSpecific.Global
{
    public class TransitionToMainMenuGloboalSceneController1 : MonoBehaviour {

        [SerializeField]
        private SceneTransitionRequest m_sceneTransitionRequest;

        private void Start()
        {
            StartCoroutine(YieldHelper.WaitForSeconds(()=> m_sceneTransitionRequest.Execute(), 17.0f));
        }
    }
}
