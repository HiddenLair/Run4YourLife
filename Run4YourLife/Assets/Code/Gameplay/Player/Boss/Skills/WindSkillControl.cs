using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;
using System;

[RequireComponent(typeof(RunnerCharacterController))]
public class WindSkillControl : MonoBehaviour {

    [SerializeField]
    private float timeToDie = 5;

    [SerializeField]
    private float windForce;

    private RunnerCharacterController m_runnerCharacterController;

    private void Awake()
    {
        m_runnerCharacterController = GetComponent<RunnerCharacterController>();
        Debug.Assert(m_runnerCharacterController != null);

        Destroy(gameObject, timeToDie);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner))
        {
            Wind wind = collider.gameObject.GetComponent<Wind>();
            if(wind == null)
            {
                wind = collider.gameObject.AddComponent<Wind>();
            }

            wind.EnterWindArea(this);
        }
    }

    public float GetWindForce()
    {
        return windForce;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner))
        {
            Wind wind = collider.gameObject.GetComponent<Wind>();
            wind.ExitWindArea(this);
        }
    }
}
