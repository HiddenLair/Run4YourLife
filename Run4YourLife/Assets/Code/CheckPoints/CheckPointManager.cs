using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {

    [SerializeField]
    private List<CheckPoint> list;
	
    public Vector3 GetPosition(float progression)
    {
        float step = 1 / list.Count;
        int index = Mathf.FloorToInt(progression/step);
    }

}
