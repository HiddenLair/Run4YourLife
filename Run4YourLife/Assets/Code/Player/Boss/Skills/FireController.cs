using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Player.Boss.Skills.Bomb
{
    [RequireComponent(typeof(AudioSource))]
    public class FireController : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("How long the player is burnt")]
        private float m_burnTime;

        [SerializeField]
        private StatusEffectSet m_burnStatusEffectSet;

        [SerializeField]
        private float minScale;

        [SerializeField]
        private float maxScale;

        private ParticleSystem[] childParticles;
        private Collider fireTrigger;

        private void Awake()
        {
            childParticles = GetComponentsInChildren<ParticleSystem>();
            fireTrigger = GetComponent<Collider>();
        }


        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Play(float growDuration, float stableDuration)
        {
            fireTrigger.enabled = true;

            Vector3 minScaleVector = new Vector3(minScale, minScale, minScale);
            Vector3 maxScaleVector = new Vector3(maxScale, maxScale, maxScale);
            SetParticlesScale(maxScaleVector);//Now particles dont scale in grow
            transform.localScale = minScaleVector;//To scale collider;

            SetEmission(true);
            StartCoroutine(Implementation(growDuration, stableDuration));
        }

        IEnumerator Implementation(float growDuration, float stableDuration)
        {
            float lifeTimeIncreasePerSec = (maxScale - minScale) / growDuration;
            float timer = Time.time + growDuration;
            Vector3 actualScale = new Vector3(minScale, minScale, minScale);
            while (timer > Time.time)
            {
                float offset = lifeTimeIncreasePerSec * Time.deltaTime;
                actualScale += new Vector3(offset, offset, offset);
                //SetParticlesScale(actualScale); 
                transform.localScale = actualScale;
                yield return null;
            }

            yield return new WaitForSeconds(stableDuration);
            Stop();
        }

        private void SetParticlesScale(Vector3 scale)
        {
            foreach (ParticleSystem p in childParticles)
            {
                p.transform.localScale = scale;
            }
        }

        private void SetEmission(bool value)
        {
            foreach (ParticleSystem p in childParticles)
            {
                ParticleSystem.EmissionModule emission = p.emission;
                emission.enabled = value;
            }
        }

        public void Stop()
        {
            SetEmission(false);
            fireTrigger.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                Burned burned = other.gameObject.GetComponent<Burned>();
                if (burned == null)
                {
                    burned = other.gameObject.AddComponent<Burned>();
                    burned.Init(m_burnTime, m_burnStatusEffectSet);
                }

                burned.RefreshTime();
            }
        }
    }
}
