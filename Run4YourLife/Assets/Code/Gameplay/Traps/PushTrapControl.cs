using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

public class PushTrapControl : MonoBehaviour {

    #region Private variables
    private bool toDelete = false;
    #endregion

    #region Public variables
    public float AOERadius = 2.0f;
    public GameObject activationParticles;
    public LayerMask trapListener;
    public LayerMask blockingElement;
    public float angleFromHorizontalAxis;
    public float pushForce;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, AOERadius, trapListener);

        foreach (Collider c in collisions)
        {
            if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
            {
                Vector3 direction = c.gameObject.GetComponent<Collider>().bounds.center - transform.position;
                bool isRight = direction.x > 0;

                ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Impulse(GetPushForce(isRight)));
                toDelete = true;
            }
        }
        
        if (toDelete)
        {
            Instantiate(activationParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    Vector3 GetPushForce(bool right)
    {
        Vector3 direction;
        if (right)
        {
            direction = Quaternion.Euler(new Vector3(0, 0, angleFromHorizontalAxis)) * Vector3.right;
        }
        else
        {
            direction = Quaternion.Euler(new Vector3(0, 0, 180.0f - angleFromHorizontalAxis)) * Vector3.right;
        }
        return direction * pushForce;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AOERadius);
        Gizmos.DrawLine(transform.position, transform.position + GetPushForce(false) / 3.0f);
        Gizmos.DrawLine(transform.position, transform.position + GetPushForce(true) / 3.0f);
    }
}
