using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CinemachineScreenTransposer : CinemachineComponentBase
{
    [Tooltip("Offset from target to be calculated from")]
    public Vector3 m_offsetFromTarget;

    [Tooltip("Meters displayed on the left side of the screen from the target. Should allways be >= 0")]
    public float m_verticalHeight = 1;

    [Range(0,1)]
    [Tooltip("Percentual screen X position for target")]
    public float m_screenX = 0.5f;

    [Range(0,1)]
    [Tooltip("Percentual screen Y position for target")]
    public float m_screenY = 0.5f;

    public override bool IsValid { get { return enabled && FollowTarget != null; } }

    public override CinemachineCore.Stage Stage { get { return CinemachineCore.Stage.Body; } }

    public override void MutateCameraState(ref CameraState curState, float deltaTime)
    {
        curState.RawPosition = CalculatePosition();
    }

    private Vector3 CalculatePosition()
    {
        Vector3 position = VirtualCamera.Follow.position + m_offsetFromTarget;


        float aspectRatio = (float)Screen.width / Screen.height;
        float xWorldDistance = (m_verticalHeight * aspectRatio) / 2.0f;
        float screenXPercentage = -m_screenX * 2 + 1; 
        position += VirtualCamera.Follow.right * xWorldDistance * screenXPercentage;

        float yWorldDistance = (m_verticalHeight / 2.0f);
        float screenYPercentage = m_screenY * 2 - 1;
        position += VirtualCamera.Follow.up * yWorldDistance * screenYPercentage;

        float zWorldDistance = m_verticalHeight / (2.0f * Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f));
        position += zWorldDistance * -VirtualCamera.Follow.forward;

        return position;
    }
}
