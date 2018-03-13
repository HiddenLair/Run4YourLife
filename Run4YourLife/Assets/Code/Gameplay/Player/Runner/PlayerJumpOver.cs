using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerJumpOver : MonoBehaviour, JumpOver {

        [SerializeField]
        private float bounceOverMeForce;
        private PlayerCharacterController characterController;

        private void Awake()
        {
            characterController = transform.parent.GetComponent<PlayerCharacterController>();
            Debug.Assert(characterController, "Objects needs a parent that has a player character controller");
        }

        #region JumpOver
        public void JumpedOn()
        {
            characterController.BounceOnMe();
        }

        public float GetBounceForce()
        {
            return bounceOverMeForce;
        }
        #endregion
    }
}
