using UnityEngine;

namespace Run4YourLife.Player
{
    public class Bullet : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("A bullet should not destroy the player but tell the player that he has been killed");

            if (other.CompareTag(Tags.Player) || other.CompareTag(Tags.Trap))
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }
        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}