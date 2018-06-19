using Run4YourLife.GameManagement.AudioManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [SerializeField]
    private FXReceiver m_destructionReceiver;

    [SerializeField]
    private FXReceiver m_baseAppearReceiver;

    [SerializeField]
    private FXReceiver m_spawnReceiver;

    [SerializeField]
    private List<Transform> pointsToFollow;

    [SerializeField]
    private float secondsOnPosition = 2.5f;

    [SerializeField]
    private float secondsTeleporting = 2.5f;

    private float timeOnPoint = 0;

    private Renderer rend;
    private Collider collider;
    private Vector3 previousPosition = new Vector3(0, 0, 0);
    private Vector3 targetPosition = new Vector3(0, 0, 0);

    private void Start()
    {
        previousPosition = new Vector3(0, 0, 0);
        rend = GetComponentInChildren<Renderer>();
        collider = GetComponentInChildren<Collider>();
        timeOnPoint = Time.time;
    }

    void Update ()
    {
		if(Time.time - timeOnPoint > secondsOnPosition + secondsTeleporting)
        {
            TeleportToNewPosition();
        }
	}

    private void TeleportToNewPosition()
    {
        previousPosition = targetPosition;
        targetPosition = pointsToFollow[Random.Range(0, pointsToFollow.Count - 1)].position;

        while (previousPosition == targetPosition)
        {
            targetPosition = pointsToFollow[Random.Range(0, pointsToFollow.Count - 1)].position;
        }

        StartCoroutine(TeleportingCoroutine(secondsTeleporting));

        gameObject.transform.position = targetPosition;

        timeOnPoint = Time.time;
    }

    private IEnumerator TeleportingCoroutine(float seconds)
    {
        m_baseAppearReceiver.PlayFx(false);
        rend.enabled = false;
        collider.enabled = false;

        yield return new WaitForSeconds(seconds);

        m_spawnReceiver.PlayFx(false);
        collider.enabled = true;
        rend.enabled = true;
    }

    private void OnEnable()
    {
        m_spawnReceiver.PlayFx(false);
    }
}
