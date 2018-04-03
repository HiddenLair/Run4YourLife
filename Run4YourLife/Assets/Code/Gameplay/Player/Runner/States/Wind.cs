using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : RunnerState, IRunnerInput
{
    private float m_windForce;

    public Wind() : base(State.Wind)
    {
    }

    public void AddWindForce(float windForce)
    {
        m_windForce += windForce;
    }

    public void RemoveWindForce(float windForce)
    {
        m_windForce -= windForce;
    }

    int IRunnerInput.GetPriority()
    {
        return 0;
    }

    public void ModifyHorizontalInput(ref float input)
    {
        input += m_windForce;
    }
}