using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGroupInfo : MonoBehaviour {

    [SerializeField]
    [Tooltip("If you need to group a single big object with multiple ones so you cant group them in hierarchy, use this")]
    private StageInfo[] referenceGroup;

    #region Variables

    private float topValue = -Mathf.Infinity;
    private float minValue = Mathf.Infinity;
    private bool minHasValue = false;

    #endregion

    private void Awake()
    {
        foreach (StageInfo info in referenceGroup)
        {
            float topValue = info.GetTopValue();
            float minValue = 0.0f;
            bool minHasValue = info.GetMinValue(out minValue);
            if (this.topValue < topValue)
            {
                this.topValue = topValue;
            }
            if (minHasValue && this.minValue > minValue)
            {
                this.minValue = minValue;
                this.minHasValue = true;
            }
            info.SetStageInfoGroup(this);
        }
    }

    public void Subscribe(float topValue, float minValue, bool minHasValue)
    {
        if(this.topValue < topValue)
        {
            this.topValue = topValue;
        }
        if(minHasValue && this.minValue > minValue)
        {
            this.minValue = minValue;
            this.minHasValue = true;
        }
    }
	
	public float GetTopValue()
    {
        return topValue;
    }

    public bool GetMinValue(out float value)
    {
        value = minValue;
        return minHasValue;
    }
}
