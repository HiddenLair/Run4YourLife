using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class ManualPrefabInstantiator : MonoBehaviour {

    #region Editor fields

    //[SerializeField]
    //private float m_timeBetweenInstantiations = 1.0f;

    //[SerializeField]
    //private bool m_isInfinite;

    //[SerializeField]
    //private int m_instantiationCount = -1;

    //[SerializeField]
    //private GameObject[] m_prefabs;

    //[SerializeField]
    //[Tooltip("How many prefabs will be pooled on awake")]
    //private int m_numberOfPrefabs;

    #endregion

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
