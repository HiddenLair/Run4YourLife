using System;
using UnityEngine;

public enum ModifierType
{
    PLAIN,
    PERCENT,
}

[Serializable]
public class StatModifier
{
    [SerializeField]
    private StatType statType;

    [SerializeField]
    private ModifierType modifierType;

    [SerializeField]
    private bool buff;

    [SerializeField]
    private float amount;

    [SerializeField]
    private float endTime;

    private Stats stats;

    private float currentTime = 0.0f;

    public StatModifier(StatType statType, ModifierType modifierType, bool buff, float amount, float endTime)
    {
        this.statType = statType;
        this.modifierType = modifierType;
        this.buff = buff;
        this.amount = amount;
        this.endTime = endTime;
    }

    public void SetStats(Stats stats)
    {
        this.stats = stats;
    }

    public bool Apply()
    {
        float value = amount;

        if(modifierType == ModifierType.PERCENT)
        {
            value *= stats.Get(statType, true);
        }

        if(!buff)
        {
            value *= -1.0f;
        }

        stats.Increase(statType, value);

        return Keep();
    }

    private bool Keep()
    {
        if(endTime >= 0.0f)
        {
            currentTime += Time.deltaTime;

            return currentTime < endTime;
        }

        return true;
    }
}