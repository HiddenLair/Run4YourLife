using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour {

    public Material mat;
    public Vector2 speed;

    private Vector2 actualTiling;
    private Vector2 initialTiling;

    private void Start()
    {
        initialTiling = mat.mainTextureOffset;
        actualTiling = mat.mainTextureOffset;
    }

    private void OnDestroy()
    {
        mat.mainTextureOffset = initialTiling;
    }

    // Update is called once per frame
    void Update () {
        mat.mainTextureOffset +=speed*Time.deltaTime;
	}
}
