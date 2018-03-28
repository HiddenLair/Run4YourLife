using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Utils;

public class DestroyAfterTime : MonoBehaviour {

    public float timeAlive = 5.0f;

    private void Start()
    {
        Destroy(gameObject, timeAlive);

        // StartCoroutine(DestroyAfterSeconds(timeAlive));
        // StartCoroutine(YieldHelper.WaitForSeconds(Destroy, gameObject, timeAlive));
    }

    /* private IEnumerator DestroyAfterSeconds(float timeAlive)
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    } */
}
