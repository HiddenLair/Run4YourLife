using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TrapType { EXPLOSION, PUSH, ROOT, DEBUFF };

public class TrapControl : MonoBehaviour
{
    #region Private variables
    private bool toDelete = false;
    #endregion

    #region Public variables
    public float AOERadius = 2.0f;
    public GameObject activationParticles;
    public LayerMask trapListener;
    public LayerMask blockingElement;
    public TrapType trapType;
    public int rootHardness = 5;
    public StatModifier statModifier;
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
                        toDelete = true;
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
                        toDelete = true;
                    }
                }
                break;

            case TrapType.ROOT:
                ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.Root(rootHardness));
                toDelete = true;
                break;

            case TrapType.DEBUFF:
                ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.Debuff(statModifier));
                toDelete = true;
                break;
        }

        if(toDelete)
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
