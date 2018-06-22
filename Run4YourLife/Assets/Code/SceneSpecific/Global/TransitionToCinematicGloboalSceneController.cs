using UnityEngine;
using UnityEngine.SceneManagement;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.SceneSpecific.Global
{
    public class TransitionToCinematicGloboalSceneController : MonoBehaviour
    {
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
}