using System;
using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    public enum AttributeType
    {
        SPEED,
        JUMP_HEIGHT,
        BOUNCE_HEIGHT
    }

    public class RunnerAttributeController : MonoBehaviour
    {
        #region InspectorVariables

        [SerializeField]
        private float m_baseSpeed;

        [SerializeField]
        private float m_jumpHeight;

        [SerializeField]
        private float m_bounceHeight;

        #endregion

        #region Private Variables

        private Dictionary<AttributeType, float> m_baseAttributes = new Dictionary<AttributeType, float>();

        private Dictionary<AttributeType, float> m_attributes = new Dictionary<AttributeType, float>();

        private List<AttributeModifier> m_attributeModifiers = new List<AttributeModifier>();

        #endregion

        void Awake()
        {
            m_baseAttributes.Add(AttributeType.SPEED, m_baseSpeed);
            m_baseAttributes.Add(AttributeType.JUMP_HEIGHT, m_jumpHeight);
            m_baseAttributes.Add(AttributeType.BOUNCE_HEIGHT, m_bounceHeight);

            ResetAttributes();
        }

        public float Get(AttributeType statType, bool initial = false)
        {
            return initial ? m_baseAttributes[statType] : m_attributes[statType];
        }

        public void Increase(AttributeType statType, float value)
        {
            m_attributes[statType] += value;
        }

        public void Set(AttributeType statType, float value)
        {
            m_attributes[statType] = value;
        }

        public void AddModifier(AttributeModifier attributeModifier)
        {
            int index = m_attributeModifiers.BinarySearch(attributeModifier);

            // If index >= 0, statModifier has already been added

            if (index < 0)
            {
                index = ~index;

                m_attributeModifiers.Insert(index, attributeModifier);
                attributeModifier.SetRunnerAttributeController(this);
            }
        }

        public void RemoveAfter(AttributeModifier attributeModifier, float time)
        {
            if (time >= 0.0f)
            {
                StartCoroutine(YieldHelper.WaitForSeconds(RemoveAttributeModifier, attributeModifier, time));
            }
        }

        public void RemoveAttributeModifier(AttributeModifier attributeModifier)
        {
            m_attributeModifiers.Remove(attributeModifier);
            RecalculateAttributes();
        }

        public void ResetAttributes()
        {
            foreach (AttributeType attributeType in Enum.GetValues(typeof(AttributeType)))
            {
                m_attributes[attributeType] = m_baseAttributes[attributeType];
            }
        }

        private void RecalculateAttributes()
        {
            ResetAttributes();

            foreach (AttributeModifier attributeModifier in m_attributeModifiers)
            {
                attributeModifier.Apply();
            }
        }
    }
}