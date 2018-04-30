using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerAttributeController))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(RunnerInputStated))]
    public class RunnerGoThroughPlatformsController : MonoBehaviour
    {
        [SerializeField]
        [Range(-1f,1f)]
        private float m_inputThreshold = -0.9f;

        private RunnerAttributeController m_runnerState;
        private Collider m_collider;
        private RunnerInputStated playerInput;

        private void Awake()
        {
            playerInput = GetComponent<RunnerInputStated>();
            m_runnerState = GetComponent<RunnerAttributeController>();
            m_collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (playerInput.GetVerticalInput() < m_inputThreshold)
            {
                PlatformGoThroughManager.Instance.IgnoreCollision(gameObject);
            }
        }
    }
}
