using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameManagement;

public class PoolRequester : MonoBehaviour {

    [SerializeField]
    private PoolRequest[] m_poolRequests;

    private void Awake()
    {
        foreach(PoolRequest poolRequest in m_poolRequests)
        {
            DynamicObjectsManager.Instance.GameObjectPool.Request(poolRequest);
        }
    }
}
