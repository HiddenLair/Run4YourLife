using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimulateChildOf : MonoBehaviour
{

    [SerializeField]
    private Transform m_parent;

    [SerializeField]
    private bool m_simulatePosition = true;

    [SerializeField]
    private bool m_simualteRotation = true;

    [SerializeField]
    public UnityEvent onParentDisabled;

    private Vector3 m_previousPosition;
    private Quaternion m_previousRotation;

    public Transform Parent
    {
        get { return m_parent; }
        set
        {
            m_parent = value;
            if (m_parent != null)
            {
                m_previousPosition = m_parent.position;
                m_previousRotation = m_parent.rotation;
            }
        }
    }

    public bool SimulatePosition { get { return m_simulatePosition; } set { m_simulatePosition = value; } }
    public bool SimulateRotation { get { return m_simualteRotation; } set { m_simualteRotation = value; } }

    private void OnEnable()
    {
        if (m_parent != null)
        {
            m_previousPosition = m_parent.position;
            m_previousRotation = m_parent.rotation;
        }
    }
    private void LateUpdate()
    {
        if (m_parent != null)
        {
            if (!m_parent.gameObject.activeSelf)
            {
                onParentDisabled.Invoke();
                m_parent = null;
                return;
            }

            if (m_simulatePosition)
            {
                Vector3 positionDelta = m_parent.position - m_previousPosition;
                transform.position += positionDelta;
            }

            if (m_simualteRotation)
            {
                Quaternion rotationDelta = Quaternion.Inverse(m_parent.rotation) * m_previousRotation;
                transform.rotation *= rotationDelta;
            }

            m_previousPosition = m_parent.position;
            m_previousRotation = m_parent.rotation;
        }
    }
}
