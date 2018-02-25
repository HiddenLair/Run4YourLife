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
            if (other.GetComponent<PlayerTopTriggerJump>())
            {
                characterController.OnPlayerHasJumpedOnTopOfAnotherPlayer();
            }
        }
    }
}