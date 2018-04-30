using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;

namespace Run4YourLife.Player
{
    public class DebuffTrapControl : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_activationParticles;

        [SerializeField]
        private AttributeStatusEffect m_attributeStatusEffect;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner) || collider.CompareTag(Tags.Shoot))
            {
                ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.StatusEffect(m_attributeStatusEffect));
                Instantiate(m_activationParticles, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}