using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Walker : MonoBehaviour {


    public float speed;
    private int id;
    private CheckPointManager checkPointManager;

    void Start()
    {
        checkPointManager = FindObjectsOfType<CheckPointManager>().Where(x => x.enabled).First();
        id = checkPointManager.Subscribe();
    }

    void Update () {
        transform.position = checkPointManager.GetPosition(id,speed);
	}
}
