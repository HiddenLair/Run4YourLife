using UnityEngine;
using Run4YourLife.UI;
using Run4YourLife.Input;
using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(PlayerControlScheme))]
    public class GeneralPlayerActions : MonoBehaviour
    {        
        private PlayerControlScheme controlScheme;
        public SceneTransitionRequest m_pauseMenuLoader;

        private void Awake()
        {
            controlScheme = GetComponent<PlayerControlScheme>();
        }

        private void Update()
        {
            VerifyActions();
        }

        private void VerifyActions()
        {
            if(controlScheme.Pause.Started())
            {
                Time.timeScale = 0;
                m_pauseMenuLoader.Execute();
            }
        }
    }
}
