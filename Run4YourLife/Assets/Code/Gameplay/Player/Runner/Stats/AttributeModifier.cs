using System;
using UnityEngine;

namespace Run4YourLife.Player
{
    public enum AttributeModifierType
    {
        PLAIN,
        PERCENT,
        SETTER,
    }

    [Serializable]
    public abstract class AttributeModifier : IComparable<AttributeModifier>
    {
        protected abstract AttributeType AttributeType { get; }

        private AttributeModifierType modifierType;
        private bool buff;
        private float amount;
        private float endTime;

        private RunnerAttributeController m_runnerAttributeController;

        protected AttributeModifier(AttributeModifierType modifierType, bool buff, float amount, float endTime)
        {
            this.modifierType = modifierType;
            this.buff = buff;
            this.amount = amount;
            this.endTime = endTime;
        }

        public void SetRunnerAttributeController(RunnerAttributeController runnerAttributeController)
        {
            this.m_runnerAttributeController = runnerAttributeController;

            Apply();

            runnerAttributeController.RemoveAfter(this, endTime);
        }

        public void Apply()
        {
            float value = amount;

            if (modifierType == AttributeModifierType.PERCENT)
            {
                value *= m_runnerAttributeController.Get(AttributeType, true);
            }

            if (!buff)
            {
                value *= -1.0f;
            }

            if (modifierType == AttributeModifierType.SETTER)
            {
                m_runnerAttributeController.Set(AttributeType, value);
            }
            else
            {
                m_runnerAttributeController.Increase(AttributeType, value);
            }

        }

        public virtual int GetPriority()
        {
            return -1;
        }

        public int CompareTo(AttributeModifier other)
        {
            int result = this.GetPriority().CompareTo(other.GetPriority());

            if (result == 0)
            {
                result = this.GetHashCode() - other.GetHashCode();
            }

            return result;
        }
    }
}