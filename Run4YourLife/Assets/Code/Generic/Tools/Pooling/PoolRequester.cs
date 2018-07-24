using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameManagement;

public class PoolRequester : MonoBehaviour
{

    private enum RequestObjective
    {
        DynamicObjectManager,
        FXManager
    }

    [SerializeField]
    private RequestObjective m_requestObjective;

    [SerializeField]
    private PoolRequest[] m_poolRequests;

    private void Awake()
    {
        if (m_requestObjective == RequestObjective.DynamicObjectManager)
        {
            foreach (PoolRequest poolRequest in m_poolRequests)
            {
                DynamicObjectsManager.Instance.GameObjectPool.Request(poolRequest);
            }
        }
        else
        {
            foreach (PoolRequest poolRequest in m_poolRequests)
            {
                FXManager.Instance.GameObjectPool.Request(poolRequest);
            }
        }
    }
}
