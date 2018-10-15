﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.Player
{
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

        public void AddAndRemoveAfterTime(StatusEffectSet statusEffect, float time)
        {
            Add(statusEffect);
            StartCoroutine(RemoveAfterTime(statusEffect, time));
        }

        private IEnumerator RemoveAfterTime(StatusEffectSet statusEffect, float time)
        {
            yield return new WaitForSeconds(time);
            Remove(statusEffect);
        }
    }
}
