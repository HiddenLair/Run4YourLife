using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Run4YourLife.SceneManagement;

public class TransitionToMainMenuGloboalSceneController : MonoBehaviour {

    [SerializeField]
    private SceneTransitionRequest m_sceneTransitionRequest;

    private void Start()
    {
        if(SceneManager.sceneCount == 1)
        {
            m_sceneTransitionRequest.Execute();
        }
    }
}
