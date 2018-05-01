using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    Attribute,
    Input
}

public abstract class StatusEffect : ScriptableObject {

    public abstract StatusEffectType StatusEffectType { get; }

    public abstract void Apply(GameObject gameObject);
    public abstract void Unapply(GameObject gameObject);
}
