using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

    public Transform center;
    public float radius;
    public Quaternion currentPosition;
    public float speed;

    private void Awake()
    {
        currentPosition = Quaternion.identity;
    }

    void Update () {
        currentPosition *= Quaternion.Euler(0, -speed*Time.deltaTime, 0);
        transform.position = center.position + currentPosition*center.right*radius;
        transform.LookAt(center);
	}
}
