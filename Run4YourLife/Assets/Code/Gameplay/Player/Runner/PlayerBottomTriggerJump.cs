using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerBottomTriggerJump : MonoBehaviour
    {
        private RunnerCharacterController characterController;

        private void Awake()
        {
            characterController = transform.parent.GetComponent<RunnerCharacterController>();
            Debug.Assert(characterController, "Objects needs a parent that has a player character controller");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (characterController.Velocity.y < 0)
            {
                JumpOver otherJumpOver = other.GetComponent<JumpOver>();
                if (otherJumpOver != null)
                {
                    characterController.Bounce(otherJumpOver.GetBounceForce());
                    otherJumpOver.JumpedOn();
                }
            }
        }
    }
}