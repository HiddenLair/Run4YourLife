using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectOnBecomeInvisible : MonoBehaviour {

    [SerializeField]
    private GameObject m_gameObjectToDisable;

    private void OnBecameInvisible()
    {
        m_gameObjectToDisable.SetActive(false);
    }
}
