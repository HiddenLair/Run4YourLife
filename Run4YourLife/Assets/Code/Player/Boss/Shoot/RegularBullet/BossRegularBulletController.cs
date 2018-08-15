using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player.Boss
{
    public class BossRegularBulletController : MonoBehaviour
    {

        [SerializeField]
        private GameObject m_hitParticles;

        [SerializeField]
        private AudioClip m_hitGroundClip;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
            {
                if (other.CompareTag(Tags.Runner))
                {
                    ExecuteEvents.Execute<IRunnerEvents>(other.gameObject, null, (x, y) => x.Kill());
                }

                FXManager.Instance.InstantiateFromValues(transform.position, Quaternion.identity, m_hitParticles);
                AudioManager.Instance.PlaySFX(m_hitGroundClip);

                gameObject.SetActive(false);
            }
        }

        private void GenerateHitParticle(Vector3 position)
        {

        }

        public void RegularBulletBecameInvisible()
        {
            gameObject.SetActive(false);
        }
    }
}