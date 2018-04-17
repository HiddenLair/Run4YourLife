using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;

public class DebuffTrapControl : MonoBehaviour {

    #region Private variables
    private bool toDelete = false;
    #endregion

    #region Public variables
    public GameObject activationParticles;
    public StatModifier statModifier;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner) || collider.CompareTag(Tags.Shoot))
        {
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.Debuff(statModifier));
            toDelete = true;
        }

        if (toDelete)
        {
            Instantiate(activationParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
