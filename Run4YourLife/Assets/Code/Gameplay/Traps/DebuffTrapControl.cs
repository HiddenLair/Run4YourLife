using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (collider.tag == Tags.Player || collider.tag == Tags.Shoot)
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
