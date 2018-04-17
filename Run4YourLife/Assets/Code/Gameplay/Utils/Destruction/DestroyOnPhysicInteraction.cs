using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPhysicInteraction : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
