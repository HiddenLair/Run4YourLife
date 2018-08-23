using System;
using UnityEngine;

namespace Run4YourLife.Player.Runner
{
    [RequireComponent(typeof(RunnerAttributeController))]
    [RequireComponent(typeof(StatusEffectController))]
    public class Burned : MonoBehaviour
    {
        private float m_burnTime;
        private float m_endBurnTime;
        private RunnerController m_runnerController;

        private StatusEffectSet m_statusEffectSet;

        private StatusEffectController m_statusEffectController;

        private void Awake()
        {
            m_statusEffectController = GetComponent<StatusEffectController>();
            Debug.Assert(m_statusEffectController != null);

            m_runnerController = GetComponent<RunnerController>();
        }

        public void Init(float burnTime, StatusEffectSet statusEffectSet)
        {
            m_burnTime = burnTime;
            m_statusEffectSet = statusEffectSet;

            m_statusEffectController.Add(statusEffectSet);

            m_runnerController.ActivateFire();
        }

        public void RefreshTime()
        {
            m_endBurnTime = Time.time + m_burnTime;
        }

        private void Update()
        {
            if (Time.time > m_endBurnTime)
            {
                EndBurned();
            }
        }

        private void OnDisable()
        {
            EndBurned();
        }

        private void EndBurned()
        {
            m_statusEffectController.Remove(m_statusEffectSet);
            m_runnerController.DeactivateFire();
            Destroy(this);
        }
    }
}