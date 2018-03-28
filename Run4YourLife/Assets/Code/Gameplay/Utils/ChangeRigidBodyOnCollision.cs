using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Utils;

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
            // StartCoroutine(ChangeDelayed());
            StartCoroutine(YieldHelper.WaitForSeconds(Change, delayActivation));
        }
        activatedFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activatedFlag)
        {
            // StartCoroutine(ChangeDelayed());
            StartCoroutine(YieldHelper.WaitForSeconds(Change, delayActivation));
        }
        activatedFlag = true;
    }

    private void Change()
    {
        body.useGravity = gravity;
        body.isKinematic = kinematic;
    }

    /* IEnumerator ChangeDelayed()
    {
        yield return new WaitForSeconds(delayActivation);
        body.useGravity = gravity;
        body.isKinematic = kinematic;
    } */
}
