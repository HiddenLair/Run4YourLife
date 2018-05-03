using UnityEngine;
using Cinemachine;
using System;
using Run4YourLife.Cinemachine;

[RequireComponent(typeof(CinemachinePath))]
[ExecuteInEditMode]
public class BossPath : MonoBehaviour {

    private CinemachinePath m_path;
    private CinemachineScreenTransposerDataHolder[] m_cinemachineScreenTransposerDataHolders;

    private void Awake()
    {
        m_path = GetComponent<CinemachinePath>();
        m_cinemachineScreenTransposerDataHolders = GetComponentsInChildren<CinemachineScreenTransposerDataHolder>();
    }

    public float NormalizeUnit(float distanceAlongPath, CinemachinePathBase.PositionUnits positionUnits)
    {
        return m_path.NormalizeUnit(distanceAlongPath, positionUnits);
    }

    public Vector3 EvaluatePositionAtUnit(float m_position, CinemachinePathBase.PositionUnits positionUnits)
    {
        return m_path.EvaluatePositionAtUnit(m_position, positionUnits);
    }

    public Quaternion EvaluateOrientationAtUnit(float position, CinemachinePathBase.PositionUnits positionUnits)
    {
        return m_path.EvaluateOrientationAtUnit(position, positionUnits);
    }

    public CinemachineScreenTransposerData EvaluateScreenTransposerDataAtUnit(float position, CinemachinePathBase.PositionUnits positionUnits)
    {
        float pathUnitsPosition = position;
        if(positionUnits == CinemachinePathBase.PositionUnits.Distance)
        {
            pathUnitsPosition = m_path.GetPathPositionFromDistance(position);
        }
        int index = (int)Math.Truncate(pathUnitsPosition);
        float decimalPart = pathUnitsPosition - index;

        CinemachineScreenTransposerData previous = m_cinemachineScreenTransposerDataHolders[index].CinemachineScreenTransposerData;
        CinemachineScreenTransposerData next = m_cinemachineScreenTransposerDataHolders[Mathf.Min(index, m_cinemachineScreenTransposerDataHolders.Length)].CinemachineScreenTransposerData;


        CinemachineScreenTransposerData blendedData = BlendCinemachineScreenTransposerData(decimalPart, previous, next);

        return blendedData;
    }

    private CinemachineScreenTransposerData BlendCinemachineScreenTransposerData(float decimalPart, CinemachineScreenTransposerData previous, CinemachineScreenTransposerData next)
    {
        //TODO blend
        return previous;
    }
}