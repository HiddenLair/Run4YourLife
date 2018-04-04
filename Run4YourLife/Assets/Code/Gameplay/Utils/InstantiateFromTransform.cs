using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFromTransform : MonoBehaviour {

    public float spawnPeriod = 1.0f;
    public int timesToInstantiate = -1; // -1 are infinite
    public GameObject toInstantiate;

    private float timer;

    private void Update()
    {
        if(timer >= spawnPeriod && (timesToInstantiate > 0 || timesToInstantiate == -1))
        {
            Instantiate(toInstantiate,transform.position,toInstantiate.transform.rotation, transform);
            if (--timesToInstantiate < -1)
            {
                timesToInstantiate = -1;
            }
            timer = 0.0f;
        }
        timer += Time.deltaTime;
    }
}
