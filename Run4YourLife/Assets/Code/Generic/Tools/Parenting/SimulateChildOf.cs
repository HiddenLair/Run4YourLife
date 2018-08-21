using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimulateChildOf : MonoBehaviour
{
    private enum OnParentDisableAction
    {
        None,
        Disable,
        UnityEventCall
    }

    [SerializeField]
    private Transform m_parent;

    [SerializeField]
    private bool m_simulatePosition = true;

    [SerializeField]
    private bool m_simualteRotation = true;

    [SerializeField]
    private OnParentDisableAction m_onParentDisableAction;

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
            ChangeBehaviourState();
        }
    }

    public bool SimulatePosition { get { return m_simulatePosition; } set { m_simulatePosition = value; } }
    public bool SimulateRotation { get { return m_simualteRotation; } set { m_simualteRotation = value; } }

    private void OnEnable()
    {
        ChangeBehaviourState();
    }

    private void ChangeBehaviourState()
    {
        if (m_parent != null)
        {
            m_previousPosition = m_parent.position;
            m_previousRotation = m_parent.rotation;
        }
    }

    private void LateUpdate()
    {
        if (m_parent != null && CheckParentActive())
        {
            if (m_simulatePosition)
            {
                Vector3 positionDelta = m_parent.position - m_previousPosition;
                transform.Translate(positionDelta, Space.World);
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

    private bool CheckParentActive()
    {
        bool active = m_parent.gameObject.activeInHierarchy;
        if (!active)
        {
            switch (m_onParentDisableAction)
            {
                case OnParentDisableAction.Disable:
                    gameObject.SetActive(false);
                    break;
                case OnParentDisableAction.UnityEventCall:
                    onParentDisabled.Invoke();
                    break;
            }
            m_parent = null;
        }
        return active;
    }

    private void OnValidate()
    {
        switch (m_onParentDisableAction)
        {
            case OnParentDisableAction.None:
            case OnParentDisableAction.Disable:
                if (onParentDisabled != null && onParentDisabled.GetPersistentEventCount() > 0)
                {
                    Debug.LogWarning("SimulateChildOf would call a unityevent with no actions but you have set some actions", gameObject);
                }
                break;

            case OnParentDisableAction.UnityEventCall:
                if (onParentDisabled.GetPersistentEventCount() == 0)
                {
                    Debug.LogWarning("SimulateChildOf would call a unityevent with no actions. It would be best if you set the ondisableaction to none", gameObject);
                }
                break;
        }
    }
}
