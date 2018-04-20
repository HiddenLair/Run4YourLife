using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Input
{
    [RequireComponent(typeof(PlayerControlScheme))]
    public class PauseEventHandler : MonoBehaviour
    {
        private GameObject m_gameManager;
        private PlayerControlScheme m_playerControlScheme;

        void Awake()
        {
            m_gameManager = GameObject.FindGameObjectWithTag(Tags.GameController);
            m_playerControlScheme = GetComponent<PlayerControlScheme>();

            PauseManager pauseManager = m_gameManager.GetComponent<PauseManager>();
            if (pauseManager != null)
            {
                pauseManager.PauseChangeEvent.AddListener(OnPauseChanged);
            } else
            {
                Debug.LogWarning("Pause Manager Not Detected, could not bound to pause events");
            }
        }

        private void Update()
        {
            if (m_playerControlScheme.Pause.Started())
            {
                ExecuteEvents.Execute<IPauseEvent>(m_gameManager, null, (x, y) => x.OnPauseInput());
            }
        }

        public void OnPauseChanged(PauseState pauseState)
        {
            m_playerControlScheme.ActionsReactOnPause(pauseState);
        }
    }
}
