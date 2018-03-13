using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PushTrapControl : MonoBehaviour {

    #region Private variables
    private bool toDelete = false;
    #endregion

    #region Public variables
    public float AOERadius = 2.0f;
    public GameObject activationParticles;
    public LayerMask trapListener;
    public LayerMask blockingElement;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, AOERadius, trapListener);

        foreach (Collider c in collisions)
        {
            if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
            {
                Vector3 direction = c.gameObject.GetComponent<Collider>().bounds.center - transform.position;
                ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Impulse(direction.normalized));
                toDelete = true;
            }
        }
        
        if (toDelete)
        {
            Instantiate(activationParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AOERadius);
    }
}
