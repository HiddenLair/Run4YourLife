using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {
    public bool enabled;
    private InputSource inputSource;

    public Action(InputSource inputSource)
    {
        this.inputSource = inputSource;
    }

    public bool Started()
    {
        return enabled && inputSource.ButtonDown();
    }

    public bool Persists()
    {
        return enabled && inputSource.Button();
    }

    public bool Ended()
    {
        return enabled && inputSource.ButtonUp();
    }

    public float Value()
    {
        return enabled ? inputSource.Value() : 0.0f;
    }
}
