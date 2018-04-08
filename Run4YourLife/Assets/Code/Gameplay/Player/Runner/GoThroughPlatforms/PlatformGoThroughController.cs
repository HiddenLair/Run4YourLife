using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameManagement;
using System;

public class PlatformGoThroughController : MonoBehaviour {

    private Collider m_collider;

    private HashSet<GameObject> ignoredRunners = new HashSet<GameObject>();

    private void Awake()
    {
        Debug.Log("Awake");

        enabled = false;

        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null);
    }

    private void LateUpdate()
    {
        foreach (GameObject runner in GameObject.FindGameObjectsWithTag(Tags.Runner))
        {
            if (!ignoredRunners.Contains(runner))
            {
                bool isRunnerOnBottom = runner.transform.position.y < transform.position.y;
                Physics.IgnoreCollision(m_collider, runner.GetComponent<Collider>(), isRunnerOnBottom);
            }
        }

        ignoredRunners.Clear();
    }

    public void IgnoreCollision(GameObject runner)
    {
        Physics.IgnoreCollision(m_collider, runner.GetComponent<Collider>());
        ignoredRunners.Add(runner);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Invisible");

        enabled = false;
    }

    private void OnBecameVisible()
    {
        Debug.Log("Visible");
        enabled = true;
    }

    
}
