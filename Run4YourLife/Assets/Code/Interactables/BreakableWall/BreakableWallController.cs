using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Player.Runner;
using System;

namespace Run4YourLife.Interactables
{
    public class BreakableWallController : MonoBehaviour, IRunnerDashBreakable, IBossSkillBreakable
    {
        [SerializeField]
        private FXReceiver m_receiver;

        [SerializeField]
        private AudioClip m_destroyClip;

        private RunnerDashBreakableState m_state;

        RunnerDashBreakableState IRunnerDashBreakable.RunnerDashBreakableState { get { return m_state; } }

        private void OnEnable()
        {
            ResetState();
        }

        private void ResetState()
        {
            m_state = RunnerDashBreakableState.Alive;
        }

        void IRunnerDashBreakable.Break()
        {
            Break();
        }

        void IBossSkillBreakable.Break()
        {
            Break();
        }

        private void Break()
        {
            m_state = RunnerDashBreakableState.Broken;

            gameObject.SetActive(false);
            m_receiver.PlayFx();
            AudioManager.Instance.PlaySFX(m_destroyClip);
        }

    }
}
