using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class BulletHitParticle : MonoBehaviour
    {

        [SerializeField]
        private GameObject hitParticles;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                GenerateHitParticle(collider.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(Tags.Runner))
            {
                GenerateHitParticle(collision.gameObject);
            }
        }

        void GenerateHitParticle(GameObject runner)
        {
            FXManager.Instance.InstantiateFromValues(runner.transform.position,Quaternion.identity,hitParticles);
        }
    }
}
