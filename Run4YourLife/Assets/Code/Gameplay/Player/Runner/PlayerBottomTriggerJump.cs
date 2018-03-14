using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerBottomTriggerJump : MonoBehaviour
    {
        private PlayerCharacterController characterController;

        private void Awake()
        {
            characterController = transform.parent.GetComponent<PlayerCharacterController>();
            Debug.Assert(characterController, "Objects needs a parent that has a player character controller");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (characterController.GetVelocity().y < 0 && other.tag == "Player")
            {
                characterController.Bounce(other.GetComponent<JumpOver>().GetBounceForce());
                other.GetComponent<JumpOver>().JumpedOn();
            }
        }
    }
}