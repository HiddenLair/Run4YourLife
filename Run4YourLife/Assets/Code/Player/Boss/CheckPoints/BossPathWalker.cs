using UnityEngine;
using Cinemachine;

using Run4YourLife.GameManagement;
using Run4YourLife.Cinemachine;

namespace Run4YourLife.Player
{
    [ExecuteInEditMode]
    public class BossPathWalker : MonoBehaviour, IProgressProvider
    {
        /// <summary>The path to follow</summary>
        [Tooltip("The path to follow")]
        public BossPath m_path;

        /// <summary>How to interpret the Path Position</summary>
        [Tooltip("How to interpret the Path Position.  If set to Path Units, values are as follows: 0 represents the first waypoint on the path, 1 is the second, and so on.  Values in-between are points on the path in between the waypoints.  If set to Distance, then Path Position represents distance along the path.")]
        public CinemachinePathBase.PositionUnits m_positionUnits = CinemachinePathBase.PositionUnits.Distance;

        /// <summary>Move the cart with this speed</summary>
        [Tooltip("Move the cart with this speed along the path.  The value is interpreted according to the Position Units setting.")]
        public float m_speed;

        [Tooltip("How the speed value changes over time")]
        public float m_acceleration;

        /// <summary>The cart's current position on the path, in distance units</summary>
        [Tooltip("The position along the path at which the cart will be placed.  This can be animated directly or, if the velocity is non-zero, will be updated automatically.  The value is interpreted according to the Position Units setting.")]
        public float m_position;

        public CinemachineScreenTransposerData CinemachineScreenTransposerData { get; private set; }

        public float Progress
        {
            get
            {
                return m_path.EvaluatePercentageAtUnit(m_position, m_positionUnits);
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdatePositionAndScreenTransposerData(m_position);
            }
            else
            {
                m_speed += m_acceleration * Time.deltaTime;
                m_position += m_speed * Time.deltaTime;
                UpdatePositionAndScreenTransposerData(m_position);
            }
        }

        private void UpdatePositionAndScreenTransposerData(float distanceAlongPath)
        {
            if (m_path != null)
            {
                m_position = m_path.NormalizeUnit(distanceAlongPath, m_positionUnits);
                transform.position = m_path.EvaluatePositionAtUnit(m_position, m_positionUnits);

                if(CameraManager.Instance.ActiveCinemachineCamera != null)
                {
                    CinemachineScreenTransposer cinemachineScreenTransposer = CameraManager.Instance.ActiveCinemachineCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineScreenTransposer>();
                    cinemachineScreenTransposer.m_cinemachineScreenTransposerData = m_path.EvaluateScreenTransposerDataAtUnit(m_position, m_positionUnits);
                }
            }
        }
    }
}