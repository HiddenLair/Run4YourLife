using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbsoluteEliminatePlayerOnTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider triggered)
    {
        if (triggered.CompareTag(Tags.Player))
        {
            ExecuteEvents.Execute<ICharacterEvents>(triggered.gameObject, null, (x, y) => x.AbsoluteKill());
        }
    }

    private void OnCollisionEnter(Collision collided)
    {
        if (collided.gameObject.CompareTag(Tags.Player))
        {
            ExecuteEvents.Execute<ICharacterEvents>(collided.gameObject, null, (x, y) => x.AbsoluteKill());
        }
    }
}
