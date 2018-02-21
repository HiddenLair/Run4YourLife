using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LethalTrap : MonoBehaviour
{
    #region Public variables
    public float areaOfEffectRadius = 2.0f;
    public LayerMask trapListener;
    public LayerMask blockingElement;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, areaOfEffectRadius, trapListener);
        foreach( Collider c in collisions )
        {
            if (!Physics.Linecast(transform.position, c.gameObject.GetComponent<Collider>().bounds.center, blockingElement))
            {
                ExecuteEvents.Execute<IEventMessageTarget>(c.gameObject, null, (x, y) => x.Explosion());
            }             
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffectRadius);
    }
}
