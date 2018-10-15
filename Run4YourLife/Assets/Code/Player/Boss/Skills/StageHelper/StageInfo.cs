﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour {

    #region Inspector

    [SerializeField]
    private bool isGround;

    #endregion

    #region Variables

    private float topValue = -Mathf.Infinity;
    private float minValue= Mathf.Infinity;
    private bool minHasValue = false;

    StageGroupInfo groupInfo;

    #endregion

    // Use this for initialization
    void Awake () {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        CalculateMinValue(colliders);
        CalculateTopValue(colliders);
        if (groupInfo == null)//We check if some group has recruided it, if not, it look if we have to subscribe
        {
            groupInfo = transform.parent.GetComponent<StageGroupInfo>();
            if (groupInfo != null)
            {
                groupInfo.Subscribe(topValue, minValue, minHasValue);
            }
        }
	}

    void CalculateMinValue(Collider[] colliders)
    {
        if (!isGround)
        {          
            foreach (Collider c in colliders)
            {
                if (!c.isTrigger)
                {
                    if(minValue > c.bounds.min.y)
                    {
                        minValue = c.bounds.min.y;
                    }
                }
            }
            minHasValue = true;
        }
    }

    void CalculateTopValue(Collider[] colliders)
    {
        foreach (Collider c in colliders)
        {
            if (!c.isTrigger)
            {
                if (topValue < c.bounds.max.y)
                {
                    topValue = c.bounds.max.y;
                }
            }
        }
    }

    public float GetTopValue()
    {
        if(groupInfo != null)
        {
            return groupInfo.GetTopValue();
        }
        return topValue;
    }

    public bool GetMinValue(out float value)
    {
        if(groupInfo != null)
        {
            return groupInfo.GetMinValue(out value);
        }
        value = minValue;
        return minHasValue;
    }

    public void SetStageInfoGroup(StageGroupInfo group)
    {
        groupInfo = group;
    }
}