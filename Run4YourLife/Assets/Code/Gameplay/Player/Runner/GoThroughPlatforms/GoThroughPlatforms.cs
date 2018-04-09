using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

public class GoThroughPlatforms : MonoBehaviour
{
    [SerializeField]
    [Range(-1f,1f)]
    private float m_inputThreshold = -0.9f;

    private Stats m_runnerState;
    private Collider m_collider;
    private RunnerInputStated playerInput;
    private PlatformGoThroughManager m_platformGoThroughManager;

    private void Awake()
    {
        playerInput = GetComponent<RunnerInputStated>();

        m_runnerState = GetComponent<Stats>();
        Debug.Assert(m_runnerState != null);

        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null);

        m_platformGoThroughManager = FindObjectOfType<PlatformGoThroughManager>();
        Debug.Assert(m_platformGoThroughManager != null);
    }

    private void Update()
    {
        if (playerInput.GetVerticalInput() < m_inputThreshold)
        {
            m_platformGoThroughManager.IgnoreCollision(gameObject);
        }
    }
}
