using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodySimpleVelocityMovement : MonoBehaviour
{

    [SerializeField]
    private Vector3 m_velocity;

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_rigidbody.MovePosition(transform.position + m_velocity * Time.deltaTime);
    }
}
