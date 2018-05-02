using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player {
    public abstract class BounceableEntityBase : MonoBehaviour {

        [SerializeField]
        private bool falling = false;

        protected abstract Vector3 BounceForce { get; }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                RunnerCharacterController characterController = other.GetComponent<RunnerCharacterController>();
                if (!falling || characterController.Velocity.y < 0)
                {
                    characterController.Bounce(BounceForce);
                    BouncedOn();
                }
            }
        }

        protected abstract void BouncedOn();
    }
}
