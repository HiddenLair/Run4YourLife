using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Interactables
{
    public class ExplosionTrapControl : TrapBase
    {
        [SerializeField]
        private float m_explosionRatius;

        [SerializeField]
        private GameObject activationParticles;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag(Tags.Runner))
            {
                Collider[] collisions = Physics.OverlapSphere(transform.position, m_explosionRatius, Layers.Runner);

                foreach (Collider c in collisions)
                {
                    if (!Physics.Linecast(transform.position, c.bounds.center, Layers.Stage))
                    {
                        ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Kill());
                    }
                }

                AudioManager.Instance.PlaySFX(m_trapDetonationClip);
                Instantiate(activationParticles, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_explosionRatius);
        }
    }
}