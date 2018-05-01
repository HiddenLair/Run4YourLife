using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour {

    public new Renderer renderer;
    public Vector2 speed;
    private bool active = true;
    private Material sharedMaterial;

    private void Awake()
    {
        sharedMaterial = renderer.sharedMaterial;
    }

    private void OnDestroy()
    {
       renderer.sharedMaterial = sharedMaterial;
    }

    // Update is called once per frame
    void Update () {
        if (active)
        {
            renderer.material.mainTextureOffset += speed * Time.deltaTime;
        }
	}

    public void SetActive(bool value)
    {
        active = value;
    }

    public bool GetActive()
    {
        return active;
    }
}
