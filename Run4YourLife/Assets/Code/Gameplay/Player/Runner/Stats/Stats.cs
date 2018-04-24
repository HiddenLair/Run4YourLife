using System;
using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Utils;

public enum StatType
{
    SPEED,
    JUMP_HEIGHT,
    BOUNCE_HEIGHT
}

public class Stats : MonoBehaviour
{
    #region InspectorVariables

    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float bounceHeight;

    #endregion

    #region Private Variables

    private class StatModifierComparer : IComparer<StatModifier>
    {
        public int Compare(StatModifier x, StatModifier y)
        {
            int result = x.GetPriority().CompareTo(y.GetPriority());

            if(result == 0)
            {
                result = x.GetHashCode() - y.GetHashCode();
            }

            return result;
        }
    }

    private Dictionary<StatType, float> initialStats = new Dictionary<StatType, float>();

    private Dictionary<StatType, float> computedStats = new Dictionary<StatType, float>();

    private List<StatModifier> statsModifiers = new List<StatModifier>();

    // private HashSet<StatModifier> statsModifiers = new HashSet<StatModifier>();

    #endregion

    void Start()
    {
        initialStats.Add(StatType.SPEED, speed);
        initialStats.Add(StatType.JUMP_HEIGHT, jumpHeight);
        initialStats.Add(StatType.BOUNCE_HEIGHT, bounceHeight);

        Clear();
    }

    public float Get(StatType statType, bool initial = false)
    {
        return initial ? initialStats[statType] : computedStats[statType];
    }

    public void Increase(StatType statType, float value)
    {
        computedStats[statType] += value;
    }

    public void Set(StatType statType, float value)
    {
        computedStats[statType] = value;
    }

    public void AddModifier(StatModifier statModifier)
    {
        int index = statsModifiers.BinarySearch(statModifier, new StatModifierComparer());

        // If index >= 0, statModifier has already been added

        if(index < 0)
        {
            index = ~index;

            statsModifiers.Insert(index, statModifier);
            statModifier.SetStats(this);
        }
    }

    public void RemoveAfter(StatModifier statModifier, float time)
    {
        if(time >= 0.0f)
        {
            StartCoroutine(YieldHelper.WaitForSeconds(RemoveStatModifier, statModifier, time));
        }
    }

    public void RemoveStatModifier(StatModifier statModifier)
    {
        statsModifiers.Remove(statModifier);
        Compute();
    }

    public void Clear()
    {
        foreach(StatType statType in Enum.GetValues(typeof(StatType)))
        {
            computedStats[statType] = initialStats[statType];
        }
    }

    private void Compute()
    {
        Clear();

        foreach(StatModifier statModifier in statsModifiers)
        {
            statModifier.Apply();
        }
    }
}