using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;
using UnityEngine.Rendering;

public class DisableByBossProximity : BossDestructedInstance {

    [SerializeField]
    private float m_alphaAnimationLenght = 0.5f;

    private Renderer[] m_renderer;

    private void Awake()
    {
        m_renderer = GetComponentsInChildren<Renderer>();
    }

    public override void OnBossDestroy()
    {
        foreach (Renderer renderer in m_renderer)
        {
            MakeTransparent(renderer.material);
            StartCoroutine(BeautifullDestroy(m_alphaAnimationLenght, renderer.material));
        }
    }

    public override void OnRegenerate()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        foreach (Renderer renderer in m_renderer)
        {
            Color color = renderer.material.color;
            color.a = 1;
            renderer.material.color = color;
            MakeOpaque(renderer.material);
        }
    }

    private IEnumerator BeautifullDestroy(float trasitionLenght, Material mat)
    {
        enabled = false;

        float time = trasitionLenght;
        Color color = mat.color;
        while (time >= 0)
        {
            color.a = time / trasitionLenght;
            mat.color = color;
            yield return null;
            time -= Time.deltaTime;
        }
        gameObject.SetActive(false);
    }

    public void MakeTransparent(Material material)
    {
        material.SetFloat("_Mode", 3);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    private void MakeOpaque(Material material)
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
}
