using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class TargetLeftSideOfScreen : MonoBehaviour {

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;
    private CinemachineComposer composer;

    [SerializeField]
    private float bossAndFloorHeight;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Debug.Assert(virtualCamera != null);

        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        Debug.Assert(transposer != null);

        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        Debug.Assert(composer != null);
    }

    private void LateUpdate()
    {
        transposer.m_FollowOffset = CalculateTransposerOffset();
        composer.m_TrackedObjectOffset = CalculateLookAtOffset();
    }

    private Vector3 CalculateTransposerOffset()
    {
        Vector3 transposerOffset = CalculateLookAtOffset();

        float zWorldDistance = bossAndFloorHeight / (2.0f * Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f));
        transposerOffset += zWorldDistance * -virtualCamera.Follow.forward;

        return transposerOffset;
    }

    private Vector3 CalculateLookAtOffset()
    {
        Vector3 lookAtOffset = Vector3.zero;

        float aspectRatio = (float)Screen.width / Screen.height;
        float xWorldDistance = (bossAndFloorHeight * aspectRatio) / 2.0f;
        lookAtOffset += virtualCamera.Follow.right * xWorldDistance;

        float yWorldDistance = (bossAndFloorHeight / 2.0f);
        lookAtOffset += virtualCamera.Follow.up * yWorldDistance;

        return lookAtOffset;
    }
}
