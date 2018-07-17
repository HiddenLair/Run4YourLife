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
                GenerateHitParticle(collider.transform.position);
            }
            else
            {
                GenerateHitParticle(transform.position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(Tags.Runner))
            {
                GenerateHitParticle(collision.transform.position);
            }
            else
            {
                GenerateHitParticle(transform.position);
            }
        }

        void GenerateHitParticle(Vector3 pos)
        {
            FXManager.Instance.InstantiateFromValues(pos, Quaternion.identity,hitParticles);
        }
    }
}
