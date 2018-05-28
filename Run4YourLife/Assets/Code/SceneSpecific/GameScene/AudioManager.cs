using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement.AudioManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        public enum Fx
        {
            RunnerFart, RunnerBounce, RunnerJump, RunnerDeath,
            TrapExplosion, TrapPush, TrapRoot, TrapSlow,
            SkillLightning, SkillFire, SkillWind, SkillWall
        };

        private AudioSource m_audioSource;

        #region Runner Audio
        public AudioClip m_runnerFart;
        public AudioClip m_runnerBounce;
        public AudioClip m_runnerJump;
        public AudioClip m_runnerDeath;
        #endregion

        #region Boss Audio

        #region Traps Audio
        public AudioClip m_explosionTrap;
        public AudioClip m_pushTrap;
        public AudioClip m_rootTrap;
        public AudioClip m_slowTrap;
        #endregion

        #region Skills Audio
        public AudioClip m_lightningSkill;
        public AudioClip m_fireSkill;
        public AudioClip m_windSkill;
        public AudioClip m_wallSkill;
        #endregion

        #endregion

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        public void PlayFX(Fx fx)
        {
            AudioClip toPlay;

            switch (fx)
            {
                case Fx.RunnerBounce:
                    toPlay = m_runnerBounce; break;
                case Fx.RunnerJump:
                    toPlay = m_runnerJump; break;
                case Fx.RunnerFart:
                    toPlay = m_runnerFart; break;
                case Fx.RunnerDeath:
                    toPlay = m_runnerDeath; break;
                case Fx.SkillFire:
                    toPlay = m_fireSkill; break;
                case Fx.SkillLightning:
                    toPlay = m_lightningSkill; break;
                case Fx.SkillWall:
                    toPlay = m_wallSkill; break;
                case Fx.SkillWind:
                    toPlay = m_windSkill; break;
                case Fx.TrapExplosion:
                    toPlay = m_explosionTrap; break;
                case Fx.TrapPush:
                    toPlay = m_pushTrap; break;
                case Fx.TrapRoot:
                    toPlay = m_rootTrap; break;
                case Fx.TrapSlow:
                    toPlay = m_slowTrap; break;
                default:
                    toPlay = m_runnerFart; break;
            }

            m_audioSource.PlayOneShot(toPlay);
        }
    }
}
