using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Player.Runner
{
    [RequireComponent(typeof(RunnerController))]
    public class Shielded : MonoBehaviour
    {

        private RunnerController m_runnerController;

        private float m_shieldEndTime;

        private void Awake()
        {
            m_runnerController = GetComponent<RunnerController>();
        }

        public void SetDuration(float duration)
        {
            m_shieldEndTime = Time.time + duration;
            m_runnerController.ActivateShield(duration);
        }

        private void Update()
        {
            if (m_shieldEndTime <= Time.time)
            {
                Destroy(this);
            }
        }

        private void OnDisable()
        {
            m_runnerController.DeactivateShield();
        }
    }
}