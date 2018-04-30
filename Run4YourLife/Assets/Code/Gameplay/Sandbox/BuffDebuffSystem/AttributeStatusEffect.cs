using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeStatusEffect", menuName = "Custom/StatusEffect/AttributeStatusEffect")]
public class AttributeStatusEffect : StatusEffect, IComparable<AttributeStatusEffect> {

    public override StatusEffectType StatusEffectType { get { return StatusEffectType.Attribute; } }

    public Attribute attribute;
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
        AttributeController attributeController = gameObject.GetComponent<AttributeController>();
        Debug.Assert(attributeController != null);
        attributeController.RecalculateAttributes();
    }
}
