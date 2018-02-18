using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImpactTrap : MonoBehaviour
{
    public float areaOfEffectRadius = 2.0f;
    public LayerMask trapListener;

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, areaOfEffectRadius, trapListener);
        foreach (Collider c in collisions)
        {
            Vector3 direction = c.gameObject.GetComponent<Collider>().bounds.center - transform.position;
            ExecuteEvents.Execute<IEventMessageTarget>(c.gameObject, null, (x, y) => x.Impulse(direction.normalized));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, areaOfEffectRadius);
    }
}
