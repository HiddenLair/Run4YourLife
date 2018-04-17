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
            if(other.tag == Tags.Runner)
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing())
                {
                    Instantiate(destroyParticles,transform.position,destroyParticles.transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
    }
}
