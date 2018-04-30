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

        [SerializeField]
        private float m_slowTime;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner) || collider.CompareTag(Tags.Shoot))
            {
                StatusEffectController statusEffectController = collider.GetComponent<StatusEffectController>();
                statusEffectController.AddAndRemoveAfterTime(m_attributeStatusEffect, m_slowTime);
                Instantiate(m_activationParticles, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}