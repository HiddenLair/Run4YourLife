using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

public class GoThroughPlatforms : MonoBehaviour
{
    [SerializeField]
    [Range(0f,1f)]
    private float m_inputThreshold = 0.9f;

    private Stats m_runnerState;
    private Collider m_collider;
    private RunnerInputStated playerInput;

    private void Awake()
    {
        playerInput = GetComponent<RunnerInputStated>();

        m_runnerState = GetComponent<Stats>();
        Debug.Assert(m_runnerState != null);

        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(Tags.DropPlatform))
        {
            if(playerInput.GetVerticalInput() > m_inputThreshold)
            {
                PlatformGoThroughController oneWayPlatform = hit.gameObject.GetComponent<PlatformGoThroughController>();
                oneWayPlatform.IgnoreCollision(gameObject);
                Physics.IgnoreCollision(m_collider, hit.collider);
            }
        }
    }
}
