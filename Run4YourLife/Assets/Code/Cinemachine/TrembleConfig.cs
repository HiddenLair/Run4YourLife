using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrembleConfig", menuName = "Custom/TrembleConfig")]
public class TrembleConfig : ScriptableObject {

    public float traumAmount;

    public float m_maxRotationalTraumaAngle;

    public float m_maxTranslationalMovement;

    public float m_frequency;

    public float m_traumaDecreaseSpeed;

    public bool useDuration;

    public float duration;
}
