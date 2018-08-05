using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Interactables
{
    public class BreakableWallController : MonoBehaviour, IRunnerDashBreakable, IBossSkillBreakable
    {
        [SerializeField]
        private FXReceiver m_receiver;

        [SerializeField]
        private AudioClip m_destroyClip;

        public void Break()
        {
            gameObject.SetActive(false);
            m_receiver.PlayFx();
            AudioManager.Instance.PlaySFX(m_destroyClip);
        }
    }
}
