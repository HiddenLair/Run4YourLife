using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RootTrapControl : MonoBehaviour {

    #region Private Variables
    private bool toDelete = false;
    #endregion

    #region Public variables
    public GameObject activationParticles;
    public int rootHardness = 5;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.Root(rootHardness));
            toDelete = true;
        }

        if (toDelete)
        {
            Instantiate(activationParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
