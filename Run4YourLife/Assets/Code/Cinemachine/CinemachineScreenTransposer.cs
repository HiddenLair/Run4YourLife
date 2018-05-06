using System;
using UnityEngine;

using Cinemachine;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Cinemachine
{
    [Serializable]
    public class CinemachineScreenTransposerData
    {
        [Tooltip("Offset from target to be calculated from")]
        public Vector3 m_offsetFromTarget;

        [Tooltip("Meters displayed on the left side of the screen from the target. Should allways be >= 0")]
        public float m_verticalHeight = 1;

        [Range(0, 1)]
        [Tooltip("Percentual screen X position for target")]
        public float m_screenX = 0.5f;

        [Range(0, 1)]
        [Tooltip("Percentual screen Y position for target")]
        public float m_screenY = 0.5f;
    }

    public class CinemachineScreenTransposer : CinemachineComponentBase
    {
        [SerializeField]
        public CinemachineScreenTransposerData m_cinemachineScreenTransposerData;

        public override bool IsValid { get { return enabled && FollowTarget != null; } }

        public override CinemachineCore.Stage Stage { get { return CinemachineCore.Stage.Body; } }

        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            if (IsValid)
            {
                curState.RawPosition = CalculatePosition();
            }
        }

        private Vector3 CalculatePosition()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 position = VirtualCamera.Follow.position + m_cinemachineScreenTransposerData.m_offsetFromTarget;

            float aspectRatio = mainCamera.aspect;
            float xWorldDistance = (m_cinemachineScreenTransposerData.m_verticalHeight * aspectRatio) / 2.0f;
            float screenXPercentage = -m_cinemachineScreenTransposerData.m_screenX * 2 + 1;
            position += VirtualCamera.Follow.right * xWorldDistance * screenXPercentage;

            float yWorldDistance = (m_cinemachineScreenTransposerData.m_verticalHeight / 2.0f);
            float screenYPercentage = m_cinemachineScreenTransposerData.m_screenY * 2 - 1;
            position += VirtualCamera.Follow.up * yWorldDistance * screenYPercentage;

            float zWorldDistance = m_cinemachineScreenTransposerData.m_verticalHeight / (2.0f * Mathf.Tan(Mathf.Deg2Rad * mainCamera.fieldOfView / 2.0f));
            position += zWorldDistance * -VirtualCamera.Follow.forward;

            return position;
        }
    }

}