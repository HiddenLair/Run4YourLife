using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StatType
{
    SPEED,
    JUMP_HEIGHT,
}

public class Stats : MonoBehaviour
{
    #region InspectorVariables

    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float TMP_currentSpeed_TMP;

    [SerializeField]
    private float TMP_currentJumpHeight_TMP;

    #endregion

    #region Private Variables

    private Dictionary<StatType, float> initialStats = new Dictionary<StatType, float>();

    private Dictionary<StatType, float> computedStats = new Dictionary<StatType, float>();

    private HashSet<StatModifier> statsModifiers = new HashSet<StatModifier>();

    #endregion

    void Start()
    {
        initialStats.Add(StatType.SPEED, speed);
        initialStats.Add(StatType.JUMP_HEIGHT, jumpHeight);

        Clear();
    }

    void Update()
    {
        TMP_currentSpeed_TMP = Get(StatType.SPEED);
        TMP_currentJumpHeight_TMP = Get(StatType.JUMP_HEIGHT);
    }

    public float Get(StatType statType, bool initial = false)
    {
        return initial ? initialStats[statType] : computedStats[statType];
    }

    public void Increase(StatType statType, float value)
    {
        computedStats[statType] += value;
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
            StartCoroutine(RemoveStatModifier(statModifier, time));
        }
    }

    private IEnumerator RemoveStatModifier(StatModifier statModifier, float time)
    {
        yield return new WaitForSeconds(time);

        statsModifiers.Remove(statModifier);

        Compute();
    }

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