using UnityEngine;

namespace Run4YourLife.Player
{
    public class ChargedBullet : MonoBehaviour
    {
        public GameObject explosion;

        private Boss callback;

        private void OnTriggerEnter(Collider other)
        {
            Explosion();
        }

        void OnBecameInvisible()
        {
            callback.SetShootStillAlive(false);
            Destroy(gameObject);
        }

        public void Explosion()
        {
            Instantiate(explosion, transform.position, transform.rotation);
            callback.SetShootStillAlive(false);
            Destroy(gameObject);
        }

        public void SetCallback(Boss boss)
        {
            callback = boss;
        }
    }
}
