using UnityEngine;
using Cinemachine;
using System;
using Run4YourLife.Cinemachine;

[RequireComponent(typeof(CinemachinePath))]
[ExecuteInEditMode]
public class BossPath : MonoBehaviour {

    [SerializeField]
    private CinemachineScreenTransposerData[] m_cinemachineScreenTransposerDatas;
    private CinemachinePath m_path;

    private void Awake()
    {
        m_path = GetComponent<CinemachinePath>();
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

        CinemachineScreenTransposerData previous = m_cinemachineScreenTransposerDatas[index];
        CinemachineScreenTransposerData next = m_cinemachineScreenTransposerDatas[Mathf.Min(index+1, m_cinemachineScreenTransposerDatas.Length -1)];

        return BlendCinemachineScreenTransposerData(decimalPart, previous, next); ;
    }

    private CinemachineScreenTransposerData BlendCinemachineScreenTransposerData(float t, CinemachineScreenTransposerData previous, CinemachineScreenTransposerData next)
    {
        return new CinemachineScreenTransposerData()
        {
            m_offsetFromTarget = Vector3.Lerp(previous.m_offsetFromTarget, next.m_offsetFromTarget, t),
            m_screenX = Mathf.Lerp(previous.m_screenX, next.m_screenX, t),
            m_screenY = Mathf.Lerp(previous.m_screenY, next.m_screenY, t),
            m_verticalHeight = Mathf.Lerp(previous.m_verticalHeight, next.m_verticalHeight, t)
        };
    }

    private void OnDrawGizmosSelected()
    {
        Debug.Assert(m_cinemachineScreenTransposerDatas.Length == m_path.m_Waypoints.Length, "The number of path positions<"+m_path.m_Waypoints.Length+ "> and path CinemachineScreenTransposerData<"+m_cinemachineScreenTransposerDatas.Length+"> does not match");
        for (int i = 0; i < m_cinemachineScreenTransposerDatas.Length; i++)
        {
            CinemachineScreenTransposerDataGizmos.DrawGizmos(m_cinemachineScreenTransposerDatas[i], m_path.m_Waypoints[i].position);
        }
    }
}