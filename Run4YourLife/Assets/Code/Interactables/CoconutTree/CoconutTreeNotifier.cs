using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutTreeNotifier : MonoBehaviour {

    private CoconutTreeController m_coconutTreeController;

    private void Awake()
    {
        m_coconutTreeController = transform.parent.GetComponent<CoconutTreeController>();
        Debug.Assert(m_coconutTreeController != null);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_coconutTreeController.OnRunnerTriggeredCoconutFall();
    }
}
