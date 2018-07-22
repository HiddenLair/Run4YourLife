using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.Player.Boss
{
    public class BossLaserBulletController : MonoBehaviour
    {
        [SerializeField]
        private float m_chargeDuration;

        [SerializeField]
        private float m_laserDuration;

        [SerializeField]
        private float m_maxLaserDistance;

        [SerializeField]
        [Tooltip("Size of the laser killing collision, imagine a X=length, Y=ratius")]
        private float m_laserCollisionWidth;

        [SerializeField]
        private FXReceiver m_chargeLaserReceiver;

        [SerializeField]
        private FXReceiver m_laserReceiver;

        private void OnEnable()
        {
            StartCoroutine(LaserBehaviour());
        }

        private void OnDisable()
        {
            ResetState();
            StopAllCoroutines();
        }

        private void ResetState()
        {
        }

        private IEnumerator LaserBehaviour()
        {
            // Charge Laser
            m_chargeLaserReceiver.PlayFx();
            yield return new WaitForSeconds(m_chargeDuration);

            // Fire Laser
            Vector3 laserEnd = LocateEndingLaserPosition();
            float laserDistance = Vector3.Distance(transform.position, laserEnd);
            
            Vector3 center = transform.position + transform.forward * laserDistance/2f;
            Vector3 halfExtents = new Vector3(m_laserCollisionWidth/2f, m_laserCollisionWidth/2f, laserDistance/2f);

            m_laserReceiver.PlayFx();

            // Check for players to kill
            Collider[] runnersHit = Physics.OverlapBox(center, halfExtents, transform.rotation, Layers.Runner, QueryTriggerInteraction.Ignore);
            foreach (Collider runnerHit in runnersHit)
            {
                IRunnerEvents runnerEvents = runnerHit.GetComponent<IRunnerEvents>();
                Debug.Assert(runnerEvents != null);
                if(runnerEvents != null)
                {
                    runnerEvents.Kill();
                }
            }

            yield return new WaitForSeconds(m_laserDuration);
            gameObject.SetActive(false);
        }

        private Vector3 LocateEndingLaserPosition()
        {
            Vector3 position;
            RaycastHit raycastHit;
            if(Physics.Raycast(transform.position, transform.forward, out raycastHit, m_maxLaserDistance, Layers.Stage, QueryTriggerInteraction.Ignore))
            {
                position = raycastHit.point;
            }
            else
            {
                position = transform.position + transform.forward * m_maxLaserDistance;
            }
            return position;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 center = transform.position + transform.forward * m_maxLaserDistance/2f;
            Vector3 size = new Vector3(m_laserCollisionWidth, m_laserCollisionWidth, m_maxLaserDistance);
            Gizmos.DrawWireCube(center, size);
        }
    }
}