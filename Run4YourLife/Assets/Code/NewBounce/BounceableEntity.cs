using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.InputManagement;

public class BounceableEntity : BounceableEntityBase {

    [SerializeField]
    [Tooltip("Allways bounce when colliding with it")]
    private bool m_allwaysBounce;

    [SerializeField]
    [Tooltip("When colliding with it check wether the runner has the jump button pressed or not. If set to positive will only jump when pressed")]
    private bool m_shouldBeJumping;

    [SerializeField]
    [Range(0,360)]
    private float m_angle;

    [SerializeField]
    private float m_bounceForce;

    private Animator m_animator;
    private Collider m_collider;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        Debug.Assert(m_animator != null);

        m_collider = GetComponentInChildren<Collider>();
        Debug.Assert(m_collider != null);
    }

    public override Vector3 BounceForce
    {
        get
        {
            return m_bounceForce * (Quaternion.Euler(0, 0, m_angle) * Vector3.right);
        }
    }

    public override void BouncedOn()
    {
        m_animator.SetTrigger("bump");
    }

    public override bool ShouldBounceByContact(RunnerCharacterController runnerCharacterController)
    {
        return m_allwaysBounce || ((!m_shouldBeJumping || m_shouldBeJumping && runnerCharacterController.GetComponent<RunnerControlScheme>().Jump.Persists()) && runnerCharacterController.Velocity.y < 0);
    }

    public override Vector3 GetStartingBouncePosition(RunnerCharacterController runnerCharacterController)
    {
        Vector3 startingBouncePosition = runnerCharacterController.transform.position;
        startingBouncePosition.y = m_collider.bounds.max.y;
        return startingBouncePosition;
    }
}
