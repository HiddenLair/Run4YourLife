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
        private GameObject gameManager = null;
        public PlayerControlScheme m_playerControlScheme;
        private bool pauseReq = false;

        void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag(Tags.GameController);
            m_playerControlScheme = GetComponent<PlayerControlScheme>();
        }

        public void Start()
        {
            gameManager.GetComponent<PauseManager>().PauseChangeEvent.AddListener(OnPauseChanged);
        }

        private void Update()
        {
            pauseReq = m_playerControlScheme.Pause.Started();

            if (pauseReq)
            {
                ExecuteEvents.Execute<IPauseEvent>(gameManager, null, (x, y) => x.OnPauseInput());
            }
        }

        public void OnPauseChanged(PauseState pauseState)
        {
            m_playerControlScheme.ActionsReactOnPause(pauseState);
        }
    }
}
