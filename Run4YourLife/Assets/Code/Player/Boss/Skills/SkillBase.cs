using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SkillBase : MonoBehaviour {

        public enum Phase {PHASE1,PHASE2,PHASE3 };

        [SerializeField]
        protected Phase phase;

        [SerializeField]
        protected AudioClip m_skillTriggerClip;

        protected AudioSource m_skillAudioSource;

        [SerializeField]
        private float m_cooldown;

        public float Cooldown { get { return m_cooldown; } }

        virtual public bool Check()
        {
            return true;
        }

        virtual protected void Reset() { }

        virtual public void StartSkill() {
            Reset();
            StartSkillImplementation();
        }
        virtual protected void StartSkillImplementation() { }
    }
}