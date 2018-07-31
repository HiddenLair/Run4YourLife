using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingGlowDefaultShader : MonoBehaviour {

    [SerializeField]
    private float minIntensity = 0;

    [SerializeField]
    private float maxIntensity = 3;

    [SerializeField]
    private float pulseSpeed = 1;

    struct RenderColor
    {
        public Material mat;
        public Color color;
        public RenderColor(Material mat, Color color)
        {
            this.mat = mat;
            this.color = color;
        }
    }
    private List<RenderColor> m_colors = new List<RenderColor>();

    void Start()
    {
        Renderer[] m_renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in m_renderers)
        {
            Material mat = renderer.material;
            if (mat.HasProperty("_EmissionColor"))
            {
                Color color = mat.GetColor("_EmissionColor");
                m_colors.Add(new RenderColor(mat,color));
            }
        }
    }

    void Update()
    {
        foreach (RenderColor item in m_colors)
        {
            float emission =minIntensity + Mathf.PingPong(Time.time * pulseSpeed, maxIntensity-minIntensity);

            Color finalColor = item.color * Mathf.LinearToGammaSpace(emission);
            item.mat.SetColor("_EmissionColor", finalColor);
        }
    }

    public void ChangeGlowColor(Color color,float time)
    {
        StartCoroutine(ColorInTime(color,time));
    }

    IEnumerator ColorInTime(Color color,float time)
    {
        float timer = Time.time + time;
        List<RenderColor> temp = new List<RenderColor>(m_colors);
        while(timer > Time.time)
        {
            for(int i = 0; i < m_colors.Count; ++i)
            {
                RenderColor c = m_colors[i];
                c.color = Color.Lerp(temp[i].color, color, 1 - (timer - Time.time / time));
                m_colors[i] = c;
            }
            yield return null;
        }
    }
}
