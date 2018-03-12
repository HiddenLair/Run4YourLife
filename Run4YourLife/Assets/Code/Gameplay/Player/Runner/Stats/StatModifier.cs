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
    #region InspectorVariables

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

    #endregion

    #region Private Variables

    private Stats stats;

    #endregion

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

        Apply();

        stats.RemoveAfter(this, endTime);
    }

    public void Apply()
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
    }
}