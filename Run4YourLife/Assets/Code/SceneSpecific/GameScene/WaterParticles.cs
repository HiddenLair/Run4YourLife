using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class WaterParticles : MonoBehaviour
    {

        [SerializeField]
        private GameObject splashParticle;

        private float maxDistanceToCheck = 5.0f;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                RaycastHit info;
                Vector3 initPos = collider.transform.position;
                initPos.y = transform.position.y;
                if (Physics.Raycast(initPos, Vector3.down, out info, maxDistanceToCheck))
                {
                    Vector3 particlePos = info.point;
                    particlePos.y = transform.position.y;
                    FXManager.Instance.InstantiateFromValues(particlePos, splashParticle.transform.rotation, splashParticle);
                }
            }
        }
    }
}
