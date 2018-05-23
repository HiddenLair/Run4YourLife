using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveWithGravity : MonoBehaviour {

    [SerializeField]
    private float m_gravity;

    public bool IsGravityEnabled { get; set; }

    private float m_yVelocity;

    private void Update()
    {
        m_yVelocity += m_gravity * Time.deltaTime;
        Vector3 position = transform.position;
        position.y += m_yVelocity * Time.deltaTime;
        transform.position = position;
    }
}
