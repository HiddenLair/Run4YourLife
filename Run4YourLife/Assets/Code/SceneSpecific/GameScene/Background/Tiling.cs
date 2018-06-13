using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Tiling : MonoBehaviour
{
    public Vector2 speed;

    private bool active = true;
    private Renderer m_renderer;
    private Material sharedMaterial;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        sharedMaterial = m_renderer.sharedMaterial;
    }

    private void OnDestroy()
    {
        m_renderer.sharedMaterial = sharedMaterial;
    }

    void Update()
    {
        if(active)
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