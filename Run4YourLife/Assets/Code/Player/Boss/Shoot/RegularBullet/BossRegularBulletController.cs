using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;

namespace Run4YourLife.Player.Boss
{
    public class BossRegularBulletController : MonoBehaviour {

        [SerializeField]
        private GameObject m_hitParticles;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                ExecuteEvents.Execute<IRunnerEvents>(other.gameObject, null, (x, y) => x.Kill());
            }
                
            GenerateHitParticle(transform.position);

            gameObject.SetActive(false);
        }

        private void GenerateHitParticle(Vector3 position)
        {
            FXManager.Instance.InstantiateFromValues(position, Quaternion.identity, m_hitParticles);        
        }

        public void RegularBulletBecameInvisible()
        {
            gameObject.SetActive(false);
        }
    }
}