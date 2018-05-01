using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player {
    public abstract class RunnerBounceDetection : MonoBehaviour {

        #region Inspector

        [SerializeField]
        private bool falling = false;

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                RunnerCharacterController characterController = other.GetComponent<RunnerCharacterController>();
                if (falling && characterController.Velocity.y < 0 || !falling)
                {
                    characterController.Bounce(GetBounceForce());
                    JumpedOn();
                }
            }
        }

        protected abstract Vector3 GetBounceForce();
        protected abstract void JumpedOn();
    }
}
