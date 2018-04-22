using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;
using UnityEngine.Rendering;

public class DisableByBossProximity : MonoBehaviour,IActivateByRender {

    private static readonly float DISTANCE_FROM_BOSSS_TO_DESTROY = 2.0f;
    private static readonly float ALPHA_TRANSITION_LENGHT = 0.5f;

    private Renderer[] m_renderer;

    private void Awake()
    {
        m_renderer = GetComponentsInChildren<Renderer>();

        enabled = false;
    }

    private void Update()
    {
        if (GameplayPlayerManager.Instance.Boss != null)
        {
            if (CheckHorizontalDistanceToBoss())
            {
                foreach(Renderer r in m_renderer) {
                    SetToTransparent(r.material);
                    StartCoroutine(BeautifullDestroy(ALPHA_TRANSITION_LENGHT, r.material));
                }
            }
        }
    }

    private bool CheckHorizontalDistanceToBoss()
    {
        foreach (Renderer r in m_renderer)
        {
            float itemPosition = transform.position.x - (r.bounds.size.x / 2.0f);
            float bossPosition = GameplayPlayerManager.Instance.Boss.transform.position.x;
            if(itemPosition - bossPosition < DISTANCE_FROM_BOSSS_TO_DESTROY)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator BeautifullDestroy(float trasitionLenght,Material mat)
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

    public void Activate()
    {
        enabled = true;
    }

    public void SetToTransparent(Material material)
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
}
