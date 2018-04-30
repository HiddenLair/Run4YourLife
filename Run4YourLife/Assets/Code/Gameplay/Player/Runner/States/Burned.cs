using System;
using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerAttributeController))]
    [RequireComponent(typeof(StatusEffectController))]
    public class Burned : MonoBehaviour
    {
        private float m_burnTime;
        private float m_endBurnTime;

        private StatusEffectSet m_statusEffectSet;

        private StatusEffectController m_statusEffectController;

        private void Awake()
        {
            m_statusEffectController = GetComponent<StatusEffectController>();
            Debug.Assert(m_statusEffectSet != null);
        }

        public void Init(float burnTime, StatusEffectSet statusEffectSet)
        {
            m_burnTime = burnTime;
            m_statusEffectSet = statusEffectSet;

            m_statusEffectController.Add(statusEffectSet);
        }

        public void RefreshTime()
        {
            m_endBurnTime = Time.time + m_burnTime;
        }

        private void Update()
        {
            if (Time.time > m_endBurnTime)
            {
                m_statusEffectController.Remove(m_statusEffectSet);
                Destroy(this);
            }
        }
    }
}