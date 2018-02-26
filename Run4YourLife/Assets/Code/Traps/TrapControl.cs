using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapControl : MonoBehaviour
{
    #region Private variables
    public enum TrapType { EXPLOSION, PUSH, ROOT };
    #endregion

    #region Public variables
    public float AOERadius = 2.0f;
    public LayerMask trapListener;
    public LayerMask blockingElement;
    public TrapType trapType;
    public int rootHardness = 5;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, AOERadius, trapListener);

        switch(trapType)
        {
            case TrapType.EXPLOSION:
                foreach (Collider c in collisions)
                {
                    if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
                    {
                        ExecuteEvents.Execute<IEventMessageTarget>(c.gameObject, null, (x, y) => x.Explosion());
                    }
                }
                break;

            case TrapType.PUSH:
                foreach (Collider c in collisions)
                {
                    if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
                    {
                        Vector3 direction = c.gameObject.GetComponent<Collider>().bounds.center - transform.position;
                        ExecuteEvents.Execute<IEventMessageTarget>(c.gameObject, null, (x, y) => x.Impulse(direction.normalized));
                    }
                }
                break;

            case TrapType.ROOT:
                foreach (Collider c in collisions)
                {
                    if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
                    {
                        ExecuteEvents.Execute<IEventMessageTarget>(c.gameObject, null, (x, y) => x.Root(rootHardness));
                    }
                }
                break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AOERadius);
    }
}
