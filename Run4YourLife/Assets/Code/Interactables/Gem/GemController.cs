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
    private FXReceiver m_baseAppearReceiver;

    [SerializeField]
    private GameObject m_graphicsChild;

    private Rigidbody m_rigidbody;
    private Collider m_collider;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetState();
    }

    public void PlaySpawn()
    {
        m_spawnReceiver.PlayFx(false);
    }

    public void TeleportToPositionAfterTime(Vector3 position, float time)
    {
        StartCoroutine(TeleportToPositionAfterTimeCoroutine(position, time));
    }

    private IEnumerator TeleportToPositionAfterTimeCoroutine(Vector3 position, float time)
    {
        m_baseAppearReceiver.PlayFx(false); // TODO What is this?

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
