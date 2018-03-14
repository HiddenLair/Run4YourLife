using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRigidBodyOnCollision : MonoBehaviour {


    [SerializeField]
    private Rigidbody body;

    public float delayActivation = 0;
    public bool gravity;
    public bool kinematic;
    //TODO:: add more options

    private bool activatedFlag=false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!activatedFlag)
        {
            StartCoroutine(ChangeDelayed());
        }
        activatedFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activatedFlag)
        {
            StartCoroutine(ChangeDelayed());
        }
        activatedFlag = true;
    }

    IEnumerator ChangeDelayed()
    {
        yield return new WaitForSeconds(delayActivation);
        body.useGravity = gravity;
        body.isKinematic = kinematic;
    }
}
