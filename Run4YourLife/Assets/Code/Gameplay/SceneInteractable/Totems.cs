using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totems : MonoBehaviour {

    public float initialFallSpeed = 5;
    public float gravity = 40;
    public float maxRotation;

    private float currentFallSpeed;
    private float currentRotation = 0.0f;
    private Collider triger;

    private void Start()
    {
        currentFallSpeed = initialFallSpeed;
        foreach(Collider c in GetComponents<Collider>())
        {
            if (c.isTrigger)
            {
                triger = c;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Runner))
        {
            triger.enabled = false;
            StartCoroutine(FallInTime());
        }
    }

    IEnumerator FallInTime()
    {
        while(currentRotation < maxRotation)
        {
            currentFallSpeed += gravity * Time.deltaTime;
            currentRotation += currentFallSpeed * Time.deltaTime;
            transform.Rotate(currentFallSpeed * Time.deltaTime,0,0);
            yield return new WaitForEndOfFrame();
        }
    }
}
