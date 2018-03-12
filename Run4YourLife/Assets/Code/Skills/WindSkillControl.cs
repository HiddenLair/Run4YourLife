using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindSkillControl : MonoBehaviour {

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.ActivateWindPush());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.DeactivateWindPush());
        }
    }
}
