using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class ManualPrefabInstantiator : MonoBehaviour
{ 
    #region Private Fields

    private GameObjectPool m_gameObjectPool;

    #endregion

    private void Awake()
    {
        m_gameObjectPool = GetComponent<GameObjectPool>();
    }

    public void ManualInstantiate(GameObject prefab, Vector3 instancePosition)
    {
        GameObject instance = m_gameObjectPool.Get(prefab);
        instance.transform.position = instancePosition;
        instance.transform.rotation = Quaternion.identity;
        instance.SetActive(true);
    }
}
