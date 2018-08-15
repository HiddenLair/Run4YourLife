using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInteractPlayFx : MonoBehaviour {

    [SerializeField]
    [Tooltip("Optional field")]
    private string tagToCheck;

    [SerializeField]
    private FXReceiver receiver;

    [SerializeField]
    private bool setParticlesParent;


    private void OnTriggerEnter(Collider other)
    {
        if(tagToCheck == null || other.tag == tagToCheck)
        {
            receiver.PlayFx(setParticlesParent);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (tagToCheck == null || collision.collider.tag == tagToCheck)
        {
            receiver.PlayFx(setParticlesParent);
        }
    }
}
