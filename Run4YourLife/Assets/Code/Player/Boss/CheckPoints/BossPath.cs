using System;

using UnityEngine;
using Cinemachine;

using Run4YourLife.Cinemachine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(CinemachinePath))]
    [ExecuteInEditMode]
    public class BossPath : MonoBehaviour
    {
        [SerializeField]
        private CinemachineScreenTransposerData[] m_cinemachineScreenTransposerDatas;
        private CinemachinePath m_path;

        private void Awake()
        {
            m_path = GetComponent<CinemachinePath>();
        }

        public float GetLength()
        {
            return m_path.PathLength;
        }

        public int GetWaypointCount()
        {
            return m_path.m_Waypoints.Length;
        }

        public Vector3 GetWaypointPosition(int index)
        {
            return m_path.m_Waypoints[index].position;
        }

        public void GetWaypointIndex(float position, CinemachinePathBase.PositionUnits positionUnits, out int index, out float decimalPart)
        {
            float pathUnitsPosition = position;

            if(positionUnits == CinemachinePathBase.PositionUnits.Distance)
            {
                pathUnitsPosition = m_path.GetPathPositionFromDistance(position);
            }

            index = (int)Math.Truncate(pathUnitsPosition);
            decimalPart = pathUnitsPosition - index;
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

        public float EvaluatePercentageAtUnit(float position, CinemachinePathBase.PositionUnits positionUnits)
        {
            return (position - m_path.MinUnit(positionUnits)) / m_path.MaxUnit(positionUnits);
        }

        public void EvaluateScreenTransposerDataAtUnit(float position, CinemachinePathBase.PositionUnits positionUnits, ref CinemachineScreenTransposerData cinemachineScreenTransposerData)
        {
            int index;
            float decimalPart;

            GetWaypointIndex(position, positionUnits, out index, out decimalPart);

            CinemachineScreenTransposerData previous = m_cinemachineScreenTransposerDatas[index];
            CinemachineScreenTransposerData next = m_cinemachineScreenTransposerDatas[Mathf.Min(index + 1, m_cinemachineScreenTransposerDatas.Length - 1)];

            BlendCinemachineScreenTransposerData(decimalPart, previous, next, ref cinemachineScreenTransposerData); ;
        }

        private void BlendCinemachineScreenTransposerData(float t, CinemachineScreenTransposerData previous, CinemachineScreenTransposerData next, ref CinemachineScreenTransposerData cinemachineScreenTransposerData)
        {
            cinemachineScreenTransposerData.m_offsetFromTarget = Vector3.Lerp(previous.m_offsetFromTarget, next.m_offsetFromTarget, t);
            cinemachineScreenTransposerData.m_screenX = Mathf.Lerp(previous.m_screenX, next.m_screenX, t);
            cinemachineScreenTransposerData.m_screenY = Mathf.Lerp(previous.m_screenY, next.m_screenY, t);
            cinemachineScreenTransposerData.m_verticalHeight = Mathf.Lerp(previous.m_verticalHeight, next.m_verticalHeight, t);
        }

        private void OnDrawGizmosSelected()
        {
            Debug.Assert(m_cinemachineScreenTransposerDatas.Length == m_path.m_Waypoints.Length, "The number of path positions<" + m_path.m_Waypoints.Length + "> and path CinemachineScreenTransposerData<" + m_cinemachineScreenTransposerDatas.Length + "> does not match");
            for (int i = 0; i < m_cinemachineScreenTransposerDatas.Length; i++)
            {
                CinemachineScreenTransposerDataGizmos.DrawGizmos(m_cinemachineScreenTransposerDatas[i], transform.position + m_path.m_Waypoints[i].position);
            }
        }
    }
}
