using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour {

    public Vector3 movement;

    private void Update()
    {
        transform.Translate(movement * Time.deltaTime);
    }
}
