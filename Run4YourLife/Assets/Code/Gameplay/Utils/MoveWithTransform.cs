using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithTransform : MonoBehaviour {

    public Vector3 speed;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed*Time.deltaTime);
	}
}
