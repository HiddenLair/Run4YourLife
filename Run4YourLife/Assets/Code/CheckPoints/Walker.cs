using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {


    public float speed;
    private int id;
    private Transform body;
    private CheckPointManager circuit;

    void Start()
    {
        body = gameObject.GetComponent<Transform>();
        circuit = CheckPointManager.Instance();
        id = circuit.Subscribe();
    }

    // Update is called once per frame
    void Update () {
        body.position = circuit.GetPosition(id,speed);
	}
}
