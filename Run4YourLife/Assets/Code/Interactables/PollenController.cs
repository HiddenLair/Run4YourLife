using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.Player.Runner;
using System;

namespace Run4YourLife.Interactables.Pollen
{
    public class PollenController : MonoBehaviour
    {
        [SerializeField]
        private Collider m_collider;

        private ParticleSystem[] m_particleSystems;

        private bool m_isDisappearing;

        private float m_endTime;

        private void Awake()
        {
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void ResetState()
        {
            m_endTime = 0f;
            m_isDisappearing = false;
            m_collider.enabled = true;
        }

        private void Disappear()
        {
            m_isDisappearing = true;
            m_collider.enabled = false;

            foreach (ParticleSystem p in m_particleSystems)
            {
                p.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        public void SetTimeAlive(float duration)
        {
            m_endTime = Time.time + duration;
        }

        private void OnCollidedWithRunner(RunnerController runnerController)
        {
            runnerController.Kill();
            Disappear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                RunnerController runnerController = other.GetComponent<RunnerController>();
                OnCollidedWithRunner(runnerController);
            }
        }

        private void Update()
        {
            if (!m_isDisappearing)
            {
                if (Time.time >= m_endTime)
                {
                    Disappear();
                }
            }
            else
            {
                if (!AreParticleSystemsAlive())
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private bool AreParticleSystemsAlive()
        {
            foreach (ParticleSystem p in m_particleSystems)
            {
                if (p.IsAlive(false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
