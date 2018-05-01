using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RunnerAttribute
{
    Speed,
    JumpHeight,
    BounceHeight
}

public enum AttributeModifierType
{
    Override,
    Plain,
    Percentual
}

[RequireComponent(typeof(StatusEffectController))]
public class RunnerAttributeController : MonoBehaviour {

    [SerializeField]
    private float m_baseSpeed;

    [SerializeField]
    private float m_baseJumpHeight;

    [SerializeField]
    private float m_baseBounceHeight;

    protected Dictionary<RunnerAttribute, float> m_baseAttributes = new Dictionary<RunnerAttribute, float>();
    private Dictionary<RunnerAttribute, float> m_attributes = new Dictionary<RunnerAttribute, float>();

    private StatusEffectController m_statusEffectController;

    private void Awake()
    {
        m_baseAttributes[RunnerAttribute.Speed] = m_baseSpeed;
        m_baseAttributes[RunnerAttribute.JumpHeight] = m_baseJumpHeight;
        m_baseAttributes[RunnerAttribute.BounceHeight] = m_baseBounceHeight;

        m_statusEffectController = GetComponent<StatusEffectController>();
        RecalculateAttributes();
    }

    private void OnEnable()
    {
        RecalculateAttributes();
    }

    public float GetBaseAttribute(RunnerAttribute attribute)
    {
        return m_baseAttributes[attribute];
    }

    public float GetAttribute(RunnerAttribute attribute)
    {
        return m_attributes[attribute];
    }

    public void RecalculateAttributes()
    {
        m_attributes = new Dictionary<RunnerAttribute, float>(m_baseAttributes);

        List<StatusEffect> statusEffects = m_statusEffectController.Get(StatusEffectType.Attribute);
        foreach(StatusEffect statusEffect in statusEffects)
        {
            AttributeStatusEffect attributeStatusEffect = (AttributeStatusEffect)statusEffect;
            ApplyStatusEffect(attributeStatusEffect);
        }
    }

    private void ApplyStatusEffect(AttributeStatusEffect attributeStatusEffect)
    {
        float value = m_baseAttributes[attributeStatusEffect.runnerAttribute];

        switch(attributeStatusEffect.attributeModifierType)
        {
            case AttributeModifierType.Override:
                value = attributeStatusEffect.value;
                break;
            case AttributeModifierType.Percentual:
                value *= attributeStatusEffect.value;
                break;
            case AttributeModifierType.Plain:
                value += attributeStatusEffect.value;
                break;
        }

        m_attributes[attributeStatusEffect.runnerAttribute] = value;
    }
}
