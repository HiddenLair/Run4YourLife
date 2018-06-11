using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife;
using Run4YourLife.Player;
using System;

[RequireComponent(typeof(RunnerCharacterController))]
public class RunnerBounceController : MonoBehaviour {

    [SerializeField]
    private float m_bounceJumpButtonSphereCastRatius;
    private RunnerCharacterController m_runnerCharacterController;

    private void Awake()
    {
        m_runnerCharacterController = GetComponent<RunnerCharacterController>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        IBounceable bounceable = other.GetComponent<IBounceable>();
        if(bounceable != null && bounceable.ShouldBounceByContact(m_runnerCharacterController))
        {
            ExecuteBounce(bounceable);
        }
    }

    public bool ExecuteBounceIfPossible()
    {
        Vector3 position = transform.position - new Vector3(0, m_bounceJumpButtonSphereCastRatius);
        Collider[] colliders = Physics.OverlapSphere(position, m_bounceJumpButtonSphereCastRatius, Layers.RunnerInteractable);
        foreach(Collider collider in colliders)
        {
            IBounceable bounceable = collider.GetComponent<IBounceable>();
            if(bounceable != null)
            {
                ExecuteBounce(bounceable);
                return true;
            }
        }

        return false;
    }
    
    private void ExecuteBounce(IBounceable bounceable)
    {
        transform.position = bounceable.GetStartingBouncePosition(m_runnerCharacterController);
        m_runnerCharacterController.Bounce(bounceable.BounceForce);
        bounceable.BouncedOn();    
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position - new Vector3(0, m_bounceJumpButtonSphereCastRatius);
        Gizmos.DrawWireSphere(position, m_bounceJumpButtonSphereCastRatius);
    }
}