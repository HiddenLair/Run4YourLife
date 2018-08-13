using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables.Pollen
{
    public class PollenController : MonoBehaviour
    {
        private float m_endTime;

        public void SetTimeAlive(float duration)
        {
            m_endTime = Time.time + duration;
        }

        public void OnCollidedWithRunner(RunnerController runnerController)
        {
            runnerController.Kill();
            Disappear();
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
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
            if (Time.time >= m_endTime)
            {
                Disappear();
            }
        }
    }
}
