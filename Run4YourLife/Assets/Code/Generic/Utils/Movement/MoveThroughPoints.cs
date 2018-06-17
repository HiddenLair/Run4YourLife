using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThroughPoints : MonoBehaviour {

    [SerializeField]
    private float m_gemSpeed = 3.0f;

    [SerializeField]
    private List<Transform> pointsToFollow;

    [SerializeField]
    private float secondsOnPosition = 2.5f;

    [SerializeField]
    private float secondsTeleporting = 2.5f;

    private float timeOnPoint = 0;

    private Renderer rend;

    private Random randomGenerator = new Random();

    private Vector3 previousPosition = new Vector3(0, 0, 0);
    private Vector3 targetPosition = new Vector3(0, 0, 0);

    private void Start()
    {
        previousPosition = new Vector3(0, 0, 0);
        rend = GetComponent<Renderer>();
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
        //Fancy disappear
        rend.enabled = false;

        yield return new WaitForSeconds(seconds);

        //Fancy appear
        rend.enabled = true;
    }
}
