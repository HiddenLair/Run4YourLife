using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UberShaderBox : MonoBehaviour {

    public Material mat;

    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        Vector3 pos = transform.position;
        mat.SetFloat("_RectangleTop", pos.y+rend.bounds.size.y/2);
        mat.SetFloat("_RectangleBot", pos.y+rend.bounds.size.y/2);
        mat.SetFloat("_RectangleX", pos.x+rend.bounds.size.x/2);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        mat.SetFloat("_RectangleTop", pos.y + rend.bounds.size.y / 2);
        mat.SetFloat("_RectangleBot", pos.y + rend.bounds.size.y / 2);
        mat.SetFloat("_RectangleX", pos.x + rend.bounds.size.x / 2);
    }
}
