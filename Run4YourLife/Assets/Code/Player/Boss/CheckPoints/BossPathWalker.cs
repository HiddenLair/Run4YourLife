using System;
using UnityEngine;
using Cinemachine;

using Run4YourLife.Cinemachine;
using Run4YourLife.GameManagement;
using Run4YourLife.Utils;

namespace Run4YourLife.Player.Boss
{
    [RequireComponent(typeof(PositionYOscilation))]
    [ExecuteInEditMode]
    public class BossPathWalker : MonoBehaviour, IProgressProvider
    {
        [Serializable]
        private class BossStatsPathModifier
        {
            public static float ExponentialPow = 0.5f;

            public enum BlendingType { None, Linear, Exponential }

            public int waypointIndex = -1;
            public float speedMultiplier = 1.0f;
            public float accelerationMultiplier = 1.0f;
            public float blendingStartingPercentage = 0.0f;
            public BlendingType blendingType = BlendingType.Exponential;
            public bool oscilationActive = true;
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

        private BossStatsPathModifier[] m_bossStatsPathModifiers;
        private PositionYOscilation oscilateScript;

        public float Progress
        {
            get
            {
                return m_path.EvaluatePercentageAtUnit(m_position, m_positionUnits);
            }
        }

        private void Awake()
        {
            oscilateScript = GetComponent<PositionYOscilation>();
            BuildBossStatsPathModifiers();
            UpdatePositionAndScreenTransposerData(m_position);
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

        private void BuildBossStatsPathModifiers()
        {
            m_bossStatsPathModifiers = new BossStatsPathModifier[m_path.GetWaypointCount()];

            for (int i = 0; i < m_bossStatsPathModifiers.Length; ++i)
            {
                m_bossStatsPathModifiers[i] = new BossStatsPathModifier();
            }

            foreach (BossStatsPathModifier bossStatsPathModifier in bossStatsPathModifiers)
            {
                Debug.Assert(bossStatsPathModifier.waypointIndex < m_bossStatsPathModifiers.Length, "BossPathWalker::BuiltBossStatsPathModifiers ... Index " + bossStatsPathModifier.waypointIndex + " not valid");

                if (m_bossStatsPathModifiers[bossStatsPathModifier.waypointIndex].waypointIndex != -1)
                {
                    Debug.LogWarning("BossPathWalker::BuiltBossStatsPathModifiers ... Index " + bossStatsPathModifier.waypointIndex + " already defined");
                }

                m_bossStatsPathModifiers[bossStatsPathModifier.waypointIndex] = bossStatsPathModifier;
            }
        }

        private void GetCurrentSpeedAndAccelerationMultipliers(out float speedMultiplier, out float accelerationMultiplier)
        {
            int index;
            float decimalPart;

            m_path.GetWaypointIndex(m_position, m_positionUnits, out index, out decimalPart);

            BossStatsPathModifier bossStatsPathModifier = m_bossStatsPathModifiers[index];

            oscilateScript.enabled = bossStatsPathModifier.oscilationActive;

            speedMultiplier = bossStatsPathModifier.speedMultiplier;
            accelerationMultiplier = bossStatsPathModifier.accelerationMultiplier;

            if (bossStatsPathModifier.blendingType != BossStatsPathModifier.BlendingType.None && bossStatsPathModifier.blendingStartingPercentage <= decimalPart && index + 1 < m_path.GetWaypointCount())
            {
                float leftWeight = 0.0f;
                float rightWeight = 0.0f;

                switch (bossStatsPathModifier.blendingType)
                {
                    case BossStatsPathModifier.BlendingType.Linear:
                        leftWeight = 1.0f - decimalPart;
                        rightWeight = decimalPart - bossStatsPathModifier.blendingStartingPercentage;

                        break;
                    case BossStatsPathModifier.BlendingType.Exponential:
                        leftWeight = Mathf.Pow(1.0f - decimalPart, BossStatsPathModifier.ExponentialPow);
                        rightWeight = Mathf.Pow(decimalPart - bossStatsPathModifier.blendingStartingPercentage, BossStatsPathModifier.ExponentialPow);

                        break;
                }

                float totalWeight = leftWeight + rightWeight;

                leftWeight /= totalWeight;
                rightWeight /= totalWeight;

                BossStatsPathModifier nextBossStatsPathModifier = m_bossStatsPathModifiers[index + 1];

                speedMultiplier = leftWeight * speedMultiplier + rightWeight * nextBossStatsPathModifier.speedMultiplier;
                accelerationMultiplier = leftWeight * accelerationMultiplier + rightWeight * nextBossStatsPathModifier.accelerationMultiplier;
            }
        }
    }
}