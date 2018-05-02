using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public FXReceiver rec;
    public GameObject fx;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rec.PlayFx(fx);
        }
        if (Input.GetKeyDown(KeyCode.B)){
            rec.PlayFxOnAnimation("wind_move",0.5f,fx);
        }
	}
}
