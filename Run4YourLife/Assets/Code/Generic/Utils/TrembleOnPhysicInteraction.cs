using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

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
                if (source.isActiveAndEnabled)
                {
                    source.PlayOneShot(collisionSound);
                }
                else
                {
                    AudioManager.Instance.PlaySFX(collisionSound);
                }
            }
        }
    }
}
