using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.InputManagement
{
    [RequireComponent(typeof(PlayerControlScheme))]
    public class PauseEventHandler : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_pauseOpenClip;

        private GameObject pauseManagerGameObject;
        private PlayerControlScheme m_playerControlScheme;

        void Awake()
        {
            pauseManagerGameObject = PauseManager.InstanceGameObject;
            m_playerControlScheme = GetComponent<PlayerControlScheme>();
        }

        void OnEnable()
        {
            ExecuteEvents.Execute<IPauseEvent>(pauseManagerGameObject, null, (x, y) => x.AttachListener(OnPauseChanged));
        }

        void OnDisable()
        {
            ExecuteEvents.Execute<IPauseEvent>(pauseManagerGameObject, null, (x, y) => x.DetachListener(OnPauseChanged));
        }

        void Update()
        {
            if(m_playerControlScheme.Pause.Started())
            {
                AudioManager.Instance.PlaySFX(m_pauseOpenClip);
                ExecuteEvents.Execute<IPauseEvent>(PauseManager.InstanceGameObject, null, (x, y) => x.OnPauseInput());
            }
        }

        public void OnPauseChanged(PauseState pauseState)
        {
            /*

            Skip one frame in order to prevent the players' input from being resolved if Pause -> Unpause

            */

            StartCoroutine(YieldHelper.SkipFrame(() => m_playerControlScheme.Active = pauseState == PauseState.UNPAUSED));
        }
    }
}