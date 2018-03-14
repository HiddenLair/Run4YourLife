using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExplosionArea : MonoBehaviour {

    public float AOERadius;
    public LayerMask layersListener;

    void Update () {
        Collider[] collisions = Physics.OverlapSphere(transform.position, AOERadius, layersListener);
        foreach (Collider c in collisions)
        {
            ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Explosion());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AOERadius);
    }
}
