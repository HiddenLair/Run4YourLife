using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThroughPoints : MonoBehaviour {

    [SerializeField]
    private float m_gemSpeed = 3.0f;

    [SerializeField]
    private List<Transform> pointsToFollow;

    private Random randomGenerator = new Random();

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private float timeWhenRecalculating;
    private float pathLength;

    private void Start()
    {
        GetNewTarget();
    }

    void Update ()
    {
		if(gameObject.transform.position != targetPosition)
        {
            float fracJourney = ((Time.time - timeWhenRecalculating) * m_gemSpeed) / pathLength;
            gameObject.transform.position = Vector3.Lerp(originalPosition, targetPosition, fracJourney);
        }
        else
        {
            GetNewTarget();
        }
	}

    private void GetNewTarget()
    {
        timeWhenRecalculating = Time.time;
        originalPosition = gameObject.transform.position;
        targetPosition = pointsToFollow[Random.Range(0, pointsToFollow.Count - 1)].position;

        pathLength = Vector3.Distance(originalPosition, targetPosition);
        //In case that the number is the same than before, this iteration will be repeated so... no problem!
    }
}
