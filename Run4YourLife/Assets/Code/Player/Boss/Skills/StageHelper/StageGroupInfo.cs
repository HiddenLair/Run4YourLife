using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGroupInfo : MonoBehaviour {

    #region Variables

    private float topValue = -Mathf.Infinity;
    private float minValue = Mathf.Infinity;
    private bool minHasValue = false;

    #endregion

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
