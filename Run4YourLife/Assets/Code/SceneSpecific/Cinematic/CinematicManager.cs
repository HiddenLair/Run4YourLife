using Run4YourLife.GameManagement.AudioManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_cinematicMusic;

    [SerializeField]
    private AudioClip m_cinematicAudioClip;

	void Start ()
    {
        if(m_cinematicMusic != null)
        {
            AudioManager.Instance.PlayMusic(m_cinematicMusic);
        }
        
        if (m_cinematicAudioClip != null)
        {
            AudioManager.Instance.PlaySFX(m_cinematicAudioClip);
        }
	}
}
