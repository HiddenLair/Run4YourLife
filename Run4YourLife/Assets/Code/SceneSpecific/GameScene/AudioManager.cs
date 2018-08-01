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

        private Coroutine m_fadeOutAndPlayMusicCoroutine;

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
            if (m_fadeOutAndPlayMusicCoroutine != null)
            {
                ResetFadeOutAndPlayMusicState();
            }

            if (m_musicAudioSource.clip != audioClip)
            {
                m_musicAudioSource.Stop();
                m_musicAudioSource.clip = audioClip;
                m_musicAudioSource.loop = true;
                m_musicAudioSource.Play();
            }
        }

        public void PlayMusicAfterFadeOut(AudioClip audioClip, float fadeOutDuration)
        {
            if (m_fadeOutAndPlayMusicCoroutine != null)
            {
                ResetFadeOutAndPlayMusicState();
            }

            m_fadeOutAndPlayMusicCoroutine = StartCoroutine(FadeOutAndPlayMusicBehaviour(audioClip, fadeOutDuration));
        }

        private void ResetFadeOutAndPlayMusicState()
        {
            StopCoroutine(m_fadeOutAndPlayMusicCoroutine);
            m_fadeOutAndPlayMusicCoroutine = null;
            m_musicAudioSource.volume = 1;
        }

        private IEnumerator FadeOutAndPlayMusicBehaviour(AudioClip audioClip, float fadeOutDuration)
        {
            float startVolume = m_musicAudioSource.volume;

            while (m_musicAudioSource.volume > 0)
            {
                m_musicAudioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
                yield return null;
            }

            m_musicAudioSource.volume = startVolume;

            PlayMusic(audioClip);
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
