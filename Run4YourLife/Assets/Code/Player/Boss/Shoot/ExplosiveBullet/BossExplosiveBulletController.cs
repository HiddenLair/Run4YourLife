using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.Player.Boss
{
    public class BossExplosiveBulletController : MonoBehaviour
    {

        [SerializeField]
        private float m_explosionRadius;

        [SerializeField]
        private float m_speed;

        [SerializeField]
        private float m_explosionDuration;

        [SerializeField]
        private FXReceiver m_explosionReceiver;

        [SerializeField]
        private GameObject m_bulletGraphics;

        private Coroutine m_explosiveBulletBehaviour;

        private Vector3 m_destiny;

        public void LaunchBullet(Vector3 destiny)
        {
            m_destiny = destiny;
            m_explosiveBulletBehaviour = StartCoroutine(ExplosiveBulletBehaviour());
        }

        private IEnumerator ExplosiveBulletBehaviour()
        {
            //Move Bullet
            while (transform.position != m_destiny)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_destiny, m_speed * Time.deltaTime);
                yield return null;
            }

            //Change graphics to explosion graphics
            m_bulletGraphics.SetActive(false);
            m_explosionReceiver.PlayFx();

            //Kill any runners in a circular area for the specified time
            float explosionEndTime = Time.time + m_explosionDuration;
            do
            {
                Collider[] collisions = Physics.OverlapSphere(transform.position, m_explosionRadius, Layers.Runner);
                foreach (Collider c in collisions)
                {
                    IRunnerEvents runnerEvents = c.GetComponent<IRunnerEvents>();
                    if (runnerEvents != null)
                    {
                        runnerEvents.Kill();
                    }
                }
                yield return null;
            } while (Time.time < explosionEndTime);

                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if(m_explosiveBulletBehaviour != null)
            {
                StopCoroutine(m_explosiveBulletBehaviour);
                m_explosiveBulletBehaviour = null;
            }

            ResetState();
        }

        private void ResetState()
        {
            m_bulletGraphics.SetActive(true);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_explosionRadius);
        }
    }
}

