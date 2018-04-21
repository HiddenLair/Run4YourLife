﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Stats))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(RunnerInputStated))]
    public class GoThroughPlatforms : MonoBehaviour
    {
        [SerializeField]
        [Range(-1f,1f)]
        private float m_inputThreshold = -0.9f;

        private Stats m_runnerState;
        private Collider m_collider;
        private RunnerInputStated playerInput;

        private void Awake()
        {
            playerInput = GetComponent<RunnerInputStated>();
            m_runnerState = GetComponent<Stats>();
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