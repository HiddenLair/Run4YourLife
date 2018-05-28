using System;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

using Run4YourLife.Cinemachine;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    [ExecuteInEditMode]
    public class BossPathWalker : MonoBehaviour, IProgressProvider
    {
        [Serializable]
        private struct BossStatsPathModifier
        {
            public int waypointIndex;
            public float speedMultiplier;
            public float accelerationMultiplier;

            public void Reset()
            {
                waypointIndex = -1;
                speedMultiplier = 1.0f;
                accelerationMultiplier = 1.0f;
            }
        }

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

        [SerializeField]
        private BossStatsPathModifier[] bossStatsPathModifiers;

        private CinemachineScreenTransposerData m_cinemachineScreenTransposerData = new CinemachineScreenTransposerData();

        public CinemachineScreenTransposerData CinemachineScreenTransposerData { get { return m_cinemachineScreenTransposerData; } }

        private List<BossStatsPathModifier> builtBossStatsPathModifiers = new List<BossStatsPathModifier>();

        public float Progress
        {
            get
            {
                return m_path.EvaluatePercentageAtUnit(m_position, m_positionUnits);
            }
        }

        private void Awake()
        {
            BuiltBossStatsPathModifiers();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdatePositionAndScreenTransposerData(m_position);
            }
            else
            {
                float currentSpeedMultiplier;
                float currentAccelerationMultiplier;

                GetCurrentSpeedAndAccelerationMultipliers(out currentSpeedMultiplier, out currentAccelerationMultiplier);

                m_speed += m_acceleration * currentAccelerationMultiplier * Time.deltaTime;
                m_position += m_speed * currentSpeedMultiplier * Time.deltaTime;

                UpdatePositionAndScreenTransposerData(m_position);
            }
        }

        private void UpdatePositionAndScreenTransposerData(float distanceAlongPath)
        {
            if (m_path != null)
            {
                m_position = m_path.NormalizeUnit(distanceAlongPath, m_positionUnits);
                transform.position = m_path.EvaluatePositionAtUnit(m_position, m_positionUnits);
                m_path.EvaluateScreenTransposerDataAtUnit(m_position, m_positionUnits, ref m_cinemachineScreenTransposerData); 

                /*if(CameraManager.Instance.ActiveCinemachineCamera != null)
                {
                    m_path.EvaluateScreenTransposerDataAtUnit(m_position, m_positionUnits, ref cinemachineScreenTransposer.m_cinemachineScreenTransposerData); 
                }*/
            }
        }

        private void BuiltBossStatsPathModifiers()
        {
            for(int i = 0; i < m_path.GetWaypointCount(); ++i)
            {
                BossStatsPathModifier bossStatsPathModifier = new BossStatsPathModifier();
                bossStatsPathModifier.Reset();

                builtBossStatsPathModifiers.Add(bossStatsPathModifier);
            }

            foreach(BossStatsPathModifier bossStatsPathModifier in bossStatsPathModifiers)
            {
                Debug.Assert(bossStatsPathModifier.waypointIndex < builtBossStatsPathModifiers.Count, "BossPathWalker::BuiltBossStatsPathModifiers ... Index " + bossStatsPathModifier.waypointIndex + " not valid");

                if(builtBossStatsPathModifiers[bossStatsPathModifier.waypointIndex].waypointIndex != -1)
                {
                    Debug.LogWarning("BossPathWalker::BuiltBossStatsPathModifiers ... Index " + bossStatsPathModifier.waypointIndex + " already defined");
                }

                builtBossStatsPathModifiers[bossStatsPathModifier.waypointIndex] = bossStatsPathModifier;
            }
        }

        private void GetCurrentSpeedAndAccelerationMultipliers(out float speedMultiplier, out float accelerationMultiplier)
        {
            int index = (int)(m_position / m_path.GetLength() * m_path.GetWaypointCount());

            BossStatsPathModifier currentBossStatsPathModifier = builtBossStatsPathModifiers[index];

            speedMultiplier = currentBossStatsPathModifier.speedMultiplier;
            accelerationMultiplier = currentBossStatsPathModifier.accelerationMultiplier;
        }
    }
}