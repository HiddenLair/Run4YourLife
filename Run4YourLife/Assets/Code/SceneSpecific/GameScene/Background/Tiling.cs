using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour {

    [UnityEngine.Serialization.FormerlySerializedAs("renderer")]
    public Renderer m_renderer;
    public Vector2 speed;
    private bool active = true;
    private Material sharedMaterial;

    private void Awake()
    {
        sharedMaterial = m_renderer.sharedMaterial;
    }

    private void OnDestroy()
    {
       m_renderer.sharedMaterial = sharedMaterial;
    }

    // Update is called once per frame
    void Update () {
        if (active)
        {
            m_renderer.material.mainTextureOffset += speed * Time.deltaTime;
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
