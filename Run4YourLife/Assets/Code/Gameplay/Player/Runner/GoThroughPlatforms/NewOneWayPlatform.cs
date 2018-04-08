using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameManagement;

public class NewOneWayPlatform : MonoBehaviour {

    private GameplayPlayerManager m_gameplayPlayerManager;
    private Collider m_collider;



    private void Awake()
    {
        //m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
        //Debug.Assert(m_gameplayPlayerManager != null);

        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null);
    }

    private void LateUpdate()
    {
        foreach (GameObject runner in GameObject.FindGameObjectsWithTag(Tags.Player))
        {
            bool isRunnerOnBottom = runner.transform.position.y < transform.position.y;
            Physics.IgnoreCollision(m_collider, runner.GetComponent<Collider>(), isRunnerOnBottom);
        }
    }
}
