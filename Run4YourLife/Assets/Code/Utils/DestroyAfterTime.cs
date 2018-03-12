using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float timeAlive = 5.0f;

    private void Start()
    {
        StartCoroutine(DestroyAfterSeconds(timeAlive));
    }

    private IEnumerator DestroyAfterSeconds(float timeAlive)
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    }
}
