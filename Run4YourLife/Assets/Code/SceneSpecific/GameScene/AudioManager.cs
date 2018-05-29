using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement.AudioManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [SerializeField]
        private AudioSource m_sfxAudioSource;

        [SerializeField]
        private AudioSource m_musicAudioSource;

        public void PlaySFX(AudioClip sfxClip)
        {
            m_sfxAudioSource.PlayOneShot(sfxClip);
        }

        public void PlayMusic(AudioClip musicClip)
        {
            StopMusic();

            m_musicAudioSource.clip = musicClip;
            m_musicAudioSource.loop = true;
            m_musicAudioSource.Play();
        }

        public void StopMusic()
        {
            m_musicAudioSource.clip = null;
            m_musicAudioSource.loop = false;
            m_musicAudioSource.Stop();
        }
    }
}
