using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour {

    private Dictionary<StatusEffectType, List<StatusEffect>> m_statusEffects;

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
    }

    public void Remove(StatusEffect statusEffect)
    {
        m_statusEffects[statusEffect.StatusEffectType].Remove(statusEffect);
        statusEffect.Unapply(gameObject);
    }

    public List<StatusEffect> Get(StatusEffectType statusEffectType)
    {
        return m_statusEffects[statusEffectType];
    }

    public void AddAndRemoveAfterTime(StatusEffect statusEffect, float time)
    {
        Add(statusEffect);
        StartCoroutine(RemoveAfterTime(statusEffect, time));
    }

    private IEnumerator RemoveAfterTime(StatusEffect statusEffect, float time)
    {
        yield return new WaitForSeconds(time);
        Remove(statusEffect);
    }
}
