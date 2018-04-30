using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class StatusEffectEvent : UnityEvent<StatusEffect> { }

public class StatusEffectController : MonoBehaviour {

    private Dictionary<StatusEffectType, List<StatusEffect>> m_statusEffects;

    public StatusEffectEvent OnStatusEffectAdded;
    public StatusEffectEvent OnStatusEffectRemoved;

    public StatusEffectController()
    {
        m_statusEffects = new Dictionary<StatusEffectType, List<StatusEffect>>();
        foreach(StatusEffectType type in Enum.GetValues(typeof(StatusEffectType)))
        {
            m_statusEffects.Add(type, new List<StatusEffect>());
        }
    }

    public void Add(StatusEffect statusEffect)
    {
        m_statusEffects[statusEffect.StatusEffectType].Add(statusEffect);
        statusEffect.Apply(gameObject);
        OnStatusEffectAdded.Invoke(statusEffect);
    }

    public void Add(StatusEffectSet statusEffectSet)
    {
        foreach(StatusEffect statusEffect in statusEffectSet.statusEffects)
        {
            Add(statusEffect);
        }
    }

    public void Remove(StatusEffect statusEffect)
    {
        m_statusEffects[statusEffect.StatusEffectType].Remove(statusEffect);
        statusEffect.Unapply(gameObject);
        OnStatusEffectRemoved.Invoke(statusEffect);
    }

    public void Remove(StatusEffectSet statusEffectSet)
    {
        foreach (StatusEffect statusEffect in statusEffectSet.statusEffects)
        {
            Remove(statusEffect);
        }
    }

    public List<StatusEffect> Get(StatusEffectType statusEffectType)
    {
        return m_statusEffects[statusEffectType];
    }
}
