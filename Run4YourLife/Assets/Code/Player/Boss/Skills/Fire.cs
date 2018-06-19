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

        [SerializeField]
        private float minScale;

        [SerializeField]
        private float maxScale;
       

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Play(float growDuration,float stableDuration)
        {
            gameObject.SetActive(true);
            transform.localScale = (new Vector3(minScale,minScale,minScale));
            StartCoroutine(Implementation(growDuration,stableDuration));
        }

        IEnumerator Implementation(float growDuration,float stableDuration)
        {
            float lifeTimeIncreasePerSec = (maxScale-minScale)/growDuration;
            float timer = Time.time + growDuration;
            Vector3 actualScale = transform.localScale;
            while (timer > Time.time)
            {
                float offset = lifeTimeIncreasePerSec * Time.deltaTime;
                actualScale += new Vector3(offset,offset,offset);
                transform.localScale = actualScale;
                yield return null;
            }

            yield return new WaitForSeconds(stableDuration);

            timer = Time.time + growDuration;
            while (timer > Time.time)
            {
                float offset = lifeTimeIncreasePerSec * Time.deltaTime;
                actualScale -= new Vector3(offset, offset, offset);
                transform.localScale = actualScale;
                yield return null;
            }
            Stop();
        }

        public void Stop()
        {
            gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
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
