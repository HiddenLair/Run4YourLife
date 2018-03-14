using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRigidBodyOnCollision : MonoBehaviour {

    public bool gravity;
    public bool kinematic;
    //TODO:: add more options

    private Rigidbody body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entra");
        body.useGravity = gravity;
        body.isKinematic = kinematic;
    }

    private void OnTriggerEnter(Collider other)
    {
        body.useGravity = gravity;
        body.isKinematic = kinematic;
    }
}
