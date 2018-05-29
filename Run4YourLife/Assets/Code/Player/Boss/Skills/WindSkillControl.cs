using System.Collections;
using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerCharacterController))]
    public class WindSkillControl : SkillBase
    {

        [SerializeField]
        private float timeToDie = 5;

        [SerializeField]
        private float windForce;

        private void Start()
        {
            m_skillAudioSource = GetComponent<AudioSource>();
            m_skillAudioSource.clip = m_skillTriggerClip;
            m_skillAudioSource.loop = true;
        }

        private void OnEnable()
        {
            m_skillAudioSource.Play();
            StartCoroutine(DeactivateAfterTime());
        }

        IEnumerator DeactivateAfterTime()
        {
            yield return new WaitForSeconds(timeToDie);
            m_skillAudioSource.Stop();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                Wind wind = collider.gameObject.GetComponent<Wind>();
                if (wind == null)
                {
                    wind = collider.gameObject.AddComponent<Wind>();
                }

                wind.EnterWindArea(this);
            }
        }

        public float GetWindForce()
        {
            return windForce;
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                Wind wind = collider.gameObject.GetComponent<Wind>();
                wind.ExitWindArea(this);
            }
        }
    }
}