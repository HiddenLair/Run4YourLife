using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteEventAfterSeconds : MonoBehaviour {

    [SerializeField]
    private UnityEvent m_toExecute;

    [SerializeField]
    private float m_seconds;

    public void Invoke()
    {
        StartCoroutine(StartEventAfterSeconds(m_seconds));
    }

    private IEnumerator StartEventAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_toExecute.Invoke();
    }
}
