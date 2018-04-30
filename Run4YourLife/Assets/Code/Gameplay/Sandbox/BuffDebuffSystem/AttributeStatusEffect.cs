using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeStatusEffect", menuName = "Custom/StatusEffect/AttributeStatusEffect")]
public class AttributeStatusEffect : StatusEffect, IComparable<AttributeStatusEffect> {

    public override StatusEffectType StatusEffectType { get { return StatusEffectType.Attribute; } }

    public RunnerAttribute runnerAttribute;
    public AttributeModifierType attributeModifierType;
    public float value;

    public int CompareTo(AttributeStatusEffect other)
    {
        if (other == null) return 1;

        return this.attributeModifierType.CompareTo(other.attributeModifierType);
    }

    public override void Apply(GameObject gameObject)
    {
        RecalculateAttributes(gameObject);
    }

    public override void Unapply(GameObject gameObject)
    {
        RecalculateAttributes(gameObject);
    }

    private void RecalculateAttributes(GameObject gameObject)
    {
        RunnerAttributeController attributeController = gameObject.GetComponent<RunnerAttributeController>();
        Debug.Assert(attributeController != null);
        attributeController.RecalculateAttributes();
    }
}
