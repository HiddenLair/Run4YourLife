﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPikeBreakController : MonoBehaviour {

    struct PartsStruct
    {
        public Vector3 initPos;
        public Quaternion initRotation;
        public GameObject part;
        public PartsStruct(Vector3 position, Quaternion rotation, GameObject part)
        {
            this.initPos = position;
            this.initRotation = rotation;
            this.part = part;
        }
    }

    [SerializeField]
    private float yForce;


    private List<PartsStruct> partsInitPositions = new List<PartsStruct>();

    private void Awake()
    {
        Transform[] transformList = GetComponentsInChildren<Transform>();
        foreach(Transform t in transformList)
        {
            partsInitPositions.Add(new PartsStruct(t.localPosition,t.localRotation,t.gameObject));
        }
    }

    private void OnEnable()
    {
        foreach(PartsStruct p in partsInitPositions)
        {
            Rigidbody body = p.part.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                body.AddForce(new Vector3(0,-yForce,0));
            }
        }
    }

    public void Reset()
    {
        foreach (PartsStruct p in partsInitPositions)
        {
            p.part.transform.localPosition = p.initPos;
            p.part.transform.localRotation = p.initRotation;
        }
    }
}
