using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player {
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Fire : MonoBehaviour {

        [SerializeField]
        [Tooltip("How long the player is burnt")]
        private float m_burnTime;

        [SerializeField]
        private StatusEffectSet m_burnStatusEffectSet;
       

        private ParticleSystem particles;
        private AudioSource source;

        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
            source = GetComponent<AudioSource>();
        }

        public void Play()
        {
            particles.Play();
            source.Play();
        }

        public void Stop()
        {
            particles.Stop();
            source.Stop();
        }

        void OnParticleCollision(GameObject other)
        {
            if(other.tag == Tags.Runner)
            {
                Burned burned = other.gameObject.GetComponent<Burned>();
                if (burned == null)
                {
                    burned = other.AddComponent<Burned>();
                    burned.Init(m_burnTime, m_burnStatusEffectSet);
                }

                burned.RefreshTime();
            }
        }
    }
}
