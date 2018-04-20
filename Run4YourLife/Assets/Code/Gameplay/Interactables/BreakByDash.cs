using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class BreakByDash : MonoBehaviour
    {
        [SerializeField]
        private GameObject destroyParticles;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing())
                {
                    Hit();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing())
                {
                    Hit();
                }
            }
        }

        private void Hit()
        {
            Instantiate(destroyParticles, transform.position, destroyParticles.transform.rotation);
            Destroy(gameObject);
        }
    }
}
