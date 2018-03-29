using System;
using UnityEngine;
using System.Collections;
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

    private Dictionary<StatType, float> initialStats = new Dictionary<StatType, float>();

    private Dictionary<StatType, float> computedStats = new Dictionary<StatType, float>();

    private HashSet<StatModifier> statsModifiers = new HashSet<StatModifier>();

    #endregion

    #region Public Variable

    public bool root;

    public bool burned;

    public bool windPush;

    public int rootHardness;

    #endregion

    void Start()
    {
        initialStats.Add(StatType.SPEED, speed);
        initialStats.Add(StatType.JUMP_HEIGHT, jumpHeight);
        initialStats.Add(StatType.BOUNCE_HEIGHT, bounceHeight);
        rootHardness = 0;

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
        statsModifiers.Add(statModifier);
        statModifier.SetStats(this);
    }

    public void RemoveAfter(StatModifier statModifier, float time)
    {
        if(time >= 0.0f)
        {
            // StartCoroutine(RemoveStatModifier(statModifier, time));
            StartCoroutine(YieldHelper.WaitForSeconds(RemoveStatModifier, statModifier, time));
        }
    }

    public void RemoveStatModifier(StatModifier statModifier)
    {
        statsModifiers.Remove(statModifier);
        Compute();
    }

    /* private IEnumerator RemoveStatModifier(StatModifier statModifier, float time)
    {
        yield return new WaitForSeconds(time);

        statsModifiers.Remove(statModifier);

        Compute();
    } */

    private void Clear()
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