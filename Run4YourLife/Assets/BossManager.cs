using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

    public Transform center;
    public float radius;
    public Quaternion currentPosition;
    public float speed;

    Vector3 startingPosition;

    bool moving = false;

    private void Awake()
    {
        currentPosition = Quaternion.identity;
        startingPosition = transform.position;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moving = !moving;
            if (!moving)
            {
                transform.position = startingPosition;
                transform.rotation = Quaternion.identity;
            }
        }

        if (moving)
        {
            currentPosition *= Quaternion.Euler(0, -speed * Time.deltaTime, 0);
            transform.position = center.position + currentPosition * center.right * radius;
            transform.LookAt(center);
        }
	}
}
