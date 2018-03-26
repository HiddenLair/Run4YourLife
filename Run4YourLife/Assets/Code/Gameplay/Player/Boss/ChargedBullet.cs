using UnityEngine;

namespace Run4YourLife.Player
{
    public class ChargedBullet : MonoBehaviour
    {
        public GameObject explosion;

        private float zValue;

        private void Update()
        {
            if(transform.position.z <= zValue)
            {
                Explosion();
            }
        }

        public void Explosion()
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        public void SetZValue(float value)
        {
            zValue = value;
        }
    }
}
