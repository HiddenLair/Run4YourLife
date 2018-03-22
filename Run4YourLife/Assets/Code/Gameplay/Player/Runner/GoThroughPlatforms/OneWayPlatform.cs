using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

    private Collider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null && !m_collider.isTrigger);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Physics.IgnoreCollision(m_collider, other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Physics.IgnoreCollision(m_collider, other, false);
        }
    }
}
