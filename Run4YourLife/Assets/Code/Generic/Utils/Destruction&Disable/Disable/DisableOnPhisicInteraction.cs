using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnPhisicInteraction : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }
}
