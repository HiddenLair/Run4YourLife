using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Player
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SkillBase : MonoBehaviour {

        [SerializeField]
        protected AudioClip m_skillTriggerClip;

        protected AudioSource m_skillAudioSource;

        [SerializeField]
        private float m_cooldown;

        public float Cooldown { get { return m_cooldown; } }
    }
}