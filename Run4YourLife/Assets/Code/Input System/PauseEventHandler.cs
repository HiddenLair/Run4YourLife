using System;
using UnityEngine;
using Run4YourLife.Input;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Input
{
    [RequireComponent(typeof(PlayerControlScheme))]
    class PauseEventHandler : MonoBehaviour
    {
        private PlayerControlScheme m_playerControlScheme;

        void Awake()
        {
            m_playerControlScheme = GetComponent<PlayerControlScheme>();
        }

        private void Update()
        {
            CheckForPauseRequest();
        }

        private void CheckForPauseRequest()
        {
            bool pauseReq = m_playerControlScheme.Pause.Started();

            if(pauseReq)
            {
                ExecuteEvents.Execute<IPauseEvent>(gameObject, null, (x, y) => x.OnPauseInput());
            }
        }
    }
}
