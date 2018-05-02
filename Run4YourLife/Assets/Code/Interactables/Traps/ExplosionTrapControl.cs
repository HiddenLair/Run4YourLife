using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class ExplosionTrapControl : MonoBehaviour
    {
        [SerializeField]
        private float m_explosionRatius;

        [SerializeField]
        public GameObject activationParticles;

        private void OnTriggerEnter(Collider collider)
        {
            Collider[] collisions = Physics.OverlapSphere(transform.position, m_explosionRatius, Layers.Runner);

            foreach (Collider c in collisions)
            {
                if (!Physics.Linecast(transform.position, c.bounds.center, Layers.Stage))
                {
                    ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Kill());
                }
            }

            Instantiate(activationParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_explosionRatius);
        }
    }
}