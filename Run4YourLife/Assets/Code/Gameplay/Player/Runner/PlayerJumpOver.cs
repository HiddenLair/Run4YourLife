using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerJumpOver : MonoBehaviour, JumpOver {

        private RunnerCharacterController characterController;

        private void Awake()
        {
            characterController = transform.parent.GetComponent<RunnerCharacterController>();
            Debug.Assert(characterController, "Objects needs a parent that has a player character controller");
        }

        #region JumpOver
        public void JumpedOn()
        {
            characterController.BounceOnMe();
        }

        public float GetBounceForce()
        {
            return GetComponentInParent<Stats>().Get(StatType.BOUNCE_HEIGHT);
        }
        #endregion
    }
}
