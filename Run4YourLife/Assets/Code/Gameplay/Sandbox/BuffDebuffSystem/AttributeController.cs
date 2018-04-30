using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
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
public class AttributeController : MonoBehaviour {

    private Dictionary<Attribute, float> m_baseAttributes = new Dictionary<Attribute, float>();
    private Dictionary<Attribute, float> m_attributes = new Dictionary<Attribute, float>();

    private StatusEffectController m_statusEffectController;

    private void Awake()
    {
        m_statusEffectController = GetComponent<StatusEffectController>();
    }

    public float GetBaseAttribute(Attribute attribute)
    {
        return m_baseAttributes[attribute];
    }

    public float GetAttribute(Attribute attribute)
    {
        return m_attributes[attribute];
    }

    public void RecalculateAttributes()
    {
        m_attributes = new Dictionary<Attribute, float>(m_baseAttributes);

        List<StatusEffect> statusEffects = m_statusEffectController.Get(StatusEffectType.Attribute);
        foreach(StatusEffect statusEffect in statusEffects)
        {
            AttributeStatusEffect attributeStatusEffect = (AttributeStatusEffect)statusEffect;
            ApplyAttributeStatusEffect(attributeStatusEffect);
        }
    }

    private void ApplyAttributeStatusEffect(AttributeStatusEffect attributeStatusEffect)
    {
        float value = m_baseAttributes[attributeStatusEffect.attribute];

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

        m_attributes[attributeStatusEffect.attribute] = value;
    }
}
