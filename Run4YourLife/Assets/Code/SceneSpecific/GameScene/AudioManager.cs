using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement.AudioManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        public enum Sfx
        {
            RunnerFart, RunnerBounce, RunnerJump, RunnerDeath,
            BossMelee, BossShoot,
            TrapExplosion, TrapPush, TrapRoot, TrapSlow,
            SkillLightning, SkillFire, SkillWind, SkillWall
        };

        public enum Music
        {
            MainMenu,
            GameScene
        };

        private AudioSource m_audioSource;

        #region Music
        public AudioClip m_mainMenuMusic;
        public AudioClip m_gameSceneMusic;
        #endregion

        #region Runner Audio
        public AudioClip m_runnerFart;
        public AudioClip m_runnerBounce;
        public AudioClip m_runnerJump;
        public AudioClip m_runnerDeath;
        #endregion

        #region Boss Audio

        public AudioClip m_bossMelee;
        public AudioClip m_bossShoot;

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

        public void PlayMusic(Music music)
        {
            StopMusic();

            AudioClip toPlay;

            switch(music)
            {
                case Music.MainMenu:
                    toPlay = m_mainMenuMusic; break;
                case Music.GameScene:
                    toPlay = m_gameSceneMusic; break;
                default:
                    toPlay = m_mainMenuMusic; break;
            }

            m_audioSource.clip = toPlay;
            m_audioSource.loop = true;
            m_audioSource.Play();
        }

        public void StopMusic()
        {
            m_audioSource.clip = null;
            m_audioSource.loop = false;
            m_audioSource.Stop();
        }

        public void PlayFX(Sfx sfx)
        {
            AudioClip toPlay;

            switch (sfx)
            {
                case Sfx.RunnerBounce:
                    toPlay = m_runnerBounce; break;
                case Sfx.RunnerJump:
                    toPlay = m_runnerJump; break;
                case Sfx.RunnerFart:
                    toPlay = m_runnerFart; break;
                case Sfx.RunnerDeath:
                    toPlay = m_runnerDeath; break;
                case Sfx.SkillFire:
                    toPlay = m_fireSkill; break;
                case Sfx.SkillLightning:
                    toPlay = m_lightningSkill; break;
                case Sfx.SkillWall:
                    toPlay = m_wallSkill; break;
                case Sfx.SkillWind:
                    toPlay = m_windSkill; break;
                case Sfx.TrapExplosion:
                    toPlay = m_explosionTrap; break;
                case Sfx.TrapPush:
                    toPlay = m_pushTrap; break;
                case Sfx.TrapRoot:
                    toPlay = m_rootTrap; break;
                case Sfx.TrapSlow:
                    toPlay = m_slowTrap; break;
                case Sfx.BossMelee:
                    toPlay = m_bossMelee; break;
                case Sfx.BossShoot:
                    toPlay = m_bossShoot; break;
                default:
                    toPlay = m_runnerFart; break;
            }

            m_audioSource.PlayOneShot(toPlay);
        }
    }
}
