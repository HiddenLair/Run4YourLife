using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), other,false);
        }
    }
}
