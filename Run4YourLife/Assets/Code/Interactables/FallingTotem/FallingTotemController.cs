using System.Collections;
using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;


namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class FallingTotemController : MonoBehaviour
    {
        [SerializeField]
        private TrembleConfig m_trembleConfig;

        [SerializeField]
        private AudioClip m_detectRunnerSound;

        [SerializeField]
        private AudioClip m_groundHitSound;

        [SerializeField]
        private float m_delayBetweenDetectionAndFall;

        [SerializeField]
        private float m_initialFallSpeed;

        [SerializeField]
        private float m_gravity;

        [SerializeField]
        private float m_endRotationX;

        [SerializeField]
        private Collider m_fallingTotemTrigger;

        [SerializeField]
        private Rigidbody m_rigidbody;

        [SerializeField]
        private FXReceiver m_dustParticles;

        private float m_rotationSpeed;

        private void Reset()
        {
            m_fallingTotemTrigger = transform.Find("TriggerFall").GetComponent<Collider>();
            m_rigidbody = GetComponent<Rigidbody>();
            m_dustParticles = transform.Find("Dust").GetComponent<FXReceiver>();
        }

        public void OnRunnerTriggeredTotemFall()
        {
            StartCoroutine(ExecuteTotemFall());
        }

        public void ResetTotem()
        {
            StopAllCoroutines();
            m_fallingTotemTrigger.enabled = true;
            transform.rotation = Quaternion.identity;
            //m_rigidbody.rotation = Quaternion.identity; // todo why does this not work?
        }

        private IEnumerator ExecuteTotemFall()
        {
            m_fallingTotemTrigger.enabled = false;

            AudioManager.Instance.PlaySFX(m_detectRunnerSound);

            yield return new WaitForSeconds(m_delayBetweenDetectionAndFall);
            
            m_rotationSpeed = m_initialFallSpeed;

            Quaternion startingRotation = m_rigidbody.rotation;
            Quaternion desiredRotation = m_rigidbody.rotation * Quaternion.Euler(m_endRotationX,0,0);

            float t = 0f;

            while (t < 1f)
            {
                Quaternion rotation = Quaternion.Lerp(startingRotation, desiredRotation, t);
                m_rigidbody.MoveRotation(rotation);
                yield return null;
                m_rotationSpeed += m_gravity * Time.deltaTime;
                t += m_rotationSpeed * Time.deltaTime;
            }

            TrembleManager.Instance.Tremble(m_trembleConfig);
            AudioManager.Instance.PlaySFX(m_groundHitSound);
            m_dustParticles.PlayFx(false);
        }

    }
}