using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LethalTrap : MonoBehaviour
{
    public float areaOfEffectRadius = 2.0f;
    public LayerMask trapListener;

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, areaOfEffectRadius, trapListener);
        foreach( Collider c in collisions )
        {
            ExecuteEvents.Execute<IEventMessageTarget>(c.gameObject, null, (x, y) => x.Explosion());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffectRadius);
    }
}
