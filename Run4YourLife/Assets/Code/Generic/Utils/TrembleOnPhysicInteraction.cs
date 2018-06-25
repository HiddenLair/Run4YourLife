using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Utils
{
    [RequireComponent(typeof(AudioSource))]
    public class TrembleOnPhysicInteraction : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layers;
        [SerializeField]
        private TrembleConfig trembleConfig;
        [SerializeField]
        private AudioClip collisionSound;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckForTremble(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckForTremble(collision.gameObject);
        }

        private void CheckForTremble(GameObject g)
        {
            if (((1 << g.layer) & layers) != 0)
            {
                TrembleManager.Instance.Tremble(trembleConfig);
                source.PlayOneShot(collisionSound);
            }
        }
    }
}
