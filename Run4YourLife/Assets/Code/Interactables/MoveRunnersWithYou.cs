using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class MoveRunnersWithYou : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                other.GetComponent<SimulateChildOf>().Parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                SimulateChildOf simulateChildOf = other.GetComponent<SimulateChildOf>();
                if (simulateChildOf.Parent == transform)
                {
                    simulateChildOf.Parent = null;
                }
            }
        }
    }
}