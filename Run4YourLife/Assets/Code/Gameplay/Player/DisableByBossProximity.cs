using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;

public class DisableByBossProximity : MonoBehaviour,IActivateByRender {

    private static readonly float DISTANCE_FROM_BOSSS_TO_DESTROY = 2.0f;
    private static readonly float ALPHA_TRANSITION_LENGHT = 0.5f;

    GameplayPlayerManager m_playerManager;
    Renderer m_renderer;

    private void Awake()
    {
        m_playerManager = FindObjectOfType<GameplayPlayerManager>();
        Debug.Assert(m_playerManager != null);
        m_renderer = GetComponentInChildren<Renderer>();

        enabled = false;
    }

    private void Update()
    {
        if (GetHorizontalDistanceToBoss() < DISTANCE_FROM_BOSSS_TO_DESTROY)
        {
            StartCoroutine(BeautifullDestroy(ALPHA_TRANSITION_LENGHT));
        }
    }

    private float GetHorizontalDistanceToBoss()
    {
        float itemPosition = transform.position.x - (m_renderer.bounds.size.x / 2.0f);
        float bossPosition = m_playerManager.Boss.transform.position.x;
        return itemPosition - bossPosition;
    }

    private IEnumerator BeautifullDestroy(float trasitionLenght)
    {
        enabled = false;

        float time = trasitionLenght;
        Material material = m_renderer.material;
        Color color = material.color;
        while (time >= 0)
        {
            color.a = time / trasitionLenght;
            material.color = color;
            yield return null;
            time -= Time.deltaTime;
        }
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        enabled = true;
    }
}
