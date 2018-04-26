using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

public class ExplosionTrapControl : MonoBehaviour {

    #region Private Variables
    private bool toDelete = false;
    #endregion

    #region Public Variables
    public float AOERadius = 2.0f;
    public LayerMask trapListener;
    public LayerMask blockingElement;
    public GameObject activationParticles;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, AOERadius, trapListener);

        foreach (Collider c in collisions)
        {
            if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
            {
                ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Kill());
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
