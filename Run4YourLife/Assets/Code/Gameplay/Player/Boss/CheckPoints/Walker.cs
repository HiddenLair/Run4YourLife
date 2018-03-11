using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {


    public float speed;
    private int id;
    private CheckPointManager checkPointManager;

    void Start()
    {
        checkPointManager = FindObjectOfType<CheckPointManager>();
        id = checkPointManager.Subscribe();
    }

    void Update () {
        transform.position = checkPointManager.GetPosition(id,speed);
	}
}
