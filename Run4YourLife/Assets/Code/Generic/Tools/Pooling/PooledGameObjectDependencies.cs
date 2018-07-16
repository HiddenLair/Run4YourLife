using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PooledGameObjectDependencies : MonoBehaviour {

    [SerializeField]
    private PoolRequest[] m_poolPetitions;

    public PoolRequest[] PoolRequests { get { return m_poolPetitions; } }

    private void OnValidate()
    {
        if(m_poolPetitions.Length == 0)
        {
            Debug.LogWarning("If pool petitions.lenght == 0 it is more performant to remove it", gameObject);
            return;
        }

        ValidatePoolRequests(m_poolPetitions);
    }

    private bool ValidatePoolRequests(PoolRequest[] poolRequests)
    {
        foreach(PoolRequest poolRequest in poolRequests)
        {
            if(poolRequest.prefab == gameObject)
            {
                Debug.LogError("A PoolRequest recursion error has been found at prefab named: " + poolRequest.prefab.name+", this means that one of the pooling descendants of the prefab wants to instantiate the prefab. This would crash the game at runtime", poolRequest.prefab);
                return false;
            }

            PooledGameObjectDependencies prefabPoolableGameObject = poolRequest.prefab.GetComponent<PooledGameObjectDependencies>();
            if (prefabPoolableGameObject != null)
            {
                if (!ValidatePoolRequests(prefabPoolableGameObject.PoolRequests))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
