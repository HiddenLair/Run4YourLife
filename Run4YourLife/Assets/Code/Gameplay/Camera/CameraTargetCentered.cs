using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetCentered : MonoBehaviour {

    [SerializeField]
    public Transform m_target;

    [SerializeField]
    private Vector3 m_targetRotation;

    [SerializeField]
    private Vector3 m_offsetFromTargetInWorldSpace;

    [SerializeField]
    private float m_visibleScreenHeight;

    private Camera m_camera;

    private void Awake()
    {
         m_camera = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, CalculateCameraPosition(), 0.8f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, CalculateCameraRotation(), 0.8f * Time.deltaTime);
    }

    private Quaternion CalculateCameraRotation()
    {
        return Quaternion.LookRotation(-(Quaternion.Euler(m_targetRotation) * m_target.forward), Vector3.up);
    }

    private Vector3 CalculateCameraPosition()
    {
        float z = m_visibleScreenHeight * Mathf.Cos(Mathf.Deg2Rad * m_camera.fieldOfView / 2.0f);
        Vector3 position = m_target.position + m_offsetFromTargetInWorldSpace;
        position += Quaternion.Euler(m_targetRotation) * m_target.forward * z;
        return position;
    }
}
