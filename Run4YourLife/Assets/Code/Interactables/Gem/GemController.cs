using Run4YourLife.GameManagement.AudioManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour, IRunnerDashBreakable
{
    [SerializeField]
    private FXReceiver m_spawnReceiver;

    [SerializeField]
    private FXReceiver m_destructionReceiver;

    [SerializeField]
    private FXReceiver m_teleportReceiver;

    [SerializeField]
    private GameObject m_graphicsChild;

    private Collider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetState();
    }

    public void TeleportToPositionAfterTime(Vector3 position, float time)
    {
        StartCoroutine(TeleportToPositionAfterTimeCoroutine(position, time));
    }

    private IEnumerator TeleportToPositionAfterTimeCoroutine(Vector3 position, float time)
    {
        m_teleportReceiver.PlayFx(false);

        m_graphicsChild.SetActive(false);
        m_collider.enabled = false;
        yield return new WaitForSeconds(time);

        transform.position = position;
        m_graphicsChild.SetActive(true);
        m_collider.enabled = true;
        
        m_spawnReceiver.PlayFx(false);
    }

    public void Break()
    {
        m_destructionReceiver.PlayFx();
        BossFightGemManager.Instance.OnGemHasBeenDestroyed();
    }

    private void ResetState()
    {
        m_graphicsChild.SetActive(true);
        m_collider.enabled = true;
    }
}
