using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class RunnerJumpOver : RunnerBounceDetection {

        private RunnerCharacterController characterController;

        private void Awake()
        {
            characterController = transform.parent.GetComponent<RunnerCharacterController>();
            Debug.Assert(characterController, "Objects needs a parent that has a player character controller");
        }

        #region JumpOver
        protected override void JumpedOn()
        {
            characterController.BounceOnMe();
        }

        protected override Vector3 GetBounceForce()
        {
            return new Vector3(0,GetComponentInParent<Stats>().Get(StatType.BOUNCE_HEIGHT),0);
        }
        #endregion
    }
}
