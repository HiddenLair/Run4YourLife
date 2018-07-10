using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Utils
{
    public class SceneMusic : MonoBehaviour {

        [SerializeField]
        private AudioClip m_musicAudioClip;

        private void Start()
        {
            AudioManager.Instance.PlayMusic(m_musicAudioClip);
        }
    }
}
