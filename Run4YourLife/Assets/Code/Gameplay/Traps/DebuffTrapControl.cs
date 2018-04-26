using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;

namespace Run4YourLife.Player
{
    public class DebuffTrapControl : MonoBehaviour
    {
        public GameObject activationParticles;
        public StatModifier statModifier;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner) || collider.CompareTag(Tags.Shoot))
            {
                ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.Debuff(statModifier));
                Instantiate(activationParticles, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}