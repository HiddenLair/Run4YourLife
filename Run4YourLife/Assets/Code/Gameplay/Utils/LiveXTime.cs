using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveXTime : MonoBehaviour {

    public float timeToLive;

	// Update is called once per frame
	void Update () {
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0)
        {
            Destroy(gameObject);
        }
	}
}
