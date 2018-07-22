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

        public void PlaySFX(AudioClip audioClip)
        {
            if (audioClip == null)
            {
                Debug.LogWarning("Trying to play null audio clip");
            }
            m_sfxAudioSource.PlayOneShot(audioClip);
        }

        public void PlayMusic(AudioClip audioClip)
        {
            if (m_musicAudioSource.clip != audioClip)
            {
                m_musicAudioSource.Stop();
                m_musicAudioSource.clip = audioClip;
                m_musicAudioSource.loop = true;
                m_musicAudioSource.Play();
            }
        }

        public void PauseMusic()
        {
            if (m_musicAudioSource.isPlaying)
            {
                m_musicAudioSource.Pause();
            }
        }

        public void UnPauseMusic()
        {
            m_musicAudioSource.UnPause();
        }

        public void StopMusic()
        {
            m_musicAudioSource.clip = null;
            m_musicAudioSource.Stop();
        }
    }
}
