using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VerticalCameraFollow : MonoBehaviour {


    #region Inspector
    [SerializeField]
    private float offset;
    #endregion
    #region variables
    private Camera camera;
    #endregion

    // Use this for initialization
    void Start () {
        camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = transform.position;
        newPos.y  = camera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(camera.transform.position.z - transform.position.z))).y;
        newPos.y -= offset;
        transform.position = newPos;
    }
}
