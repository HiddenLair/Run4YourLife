using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;

public class DestroyByBossProximity : MonoBehaviour {

    const float distanceFromBossToBeDestroyed = 2.0f;

    #region Variables

    GameplayPlayerManager playerManager;
    bool active = false;

    #endregion

    private void Awake()
    {
        playerManager = FindObjectOfType<GameplayPlayerManager>();
    }

    private void OnBecameVisible()
    {
        active = true;
    }

    private void Update()
    {
        if (active)
        {
            float limitPosX = playerManager.Boss.transform.position.x + distanceFromBossToBeDestroyed;
            float itemPosX = transform.position.x - GetComponent<Renderer>().bounds.size.x / 2;
            if (limitPosX > itemPosX)
            {
                StartCoroutine(BeautifullDestroy(0.5f));
            }
        }
    }

    IEnumerator BeautifullDestroy(float delay)
    {
        float fps = 1 / Time.deltaTime;
        float alphaPerFrame = 1 / (delay * fps);
        Color temp = GetComponentInChildren<Renderer>().material.color;
        while (temp.a > 0)
        {
            temp.a -= alphaPerFrame;
            GetComponentInChildren<Renderer>().material.color = temp;
            yield return 0;
        }
        Destroy(gameObject);
    }

}
