using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Input
{
    [RequireComponent(typeof(PlayerControlScheme))]
    public class PauseEventHandler : MonoBehaviour
    {
        private PlayerControlScheme m_playerControlScheme;

        void Awake()
        {
            m_playerControlScheme = GetComponent<PlayerControlScheme>();
        }

        private void Update()
        {
            if (m_playerControlScheme.Pause.Started())
            {
                ExecuteEvents.Execute<IPauseEvent>(PauseManager.InstanceGameObject, null, (x, y) => x.OnPauseInput());
            }
        }

        public void OnPauseChanged(PauseState pauseState)
        {
            m_playerControlScheme.Active = pauseState == PauseState.UNPAUSED;
        }
    }
}
