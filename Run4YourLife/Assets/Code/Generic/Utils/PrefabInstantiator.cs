using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class PrefabInstantiator : MonoBehaviour {

    #region Editor fields

    [SerializeField]
    private float m_timeBetweenInstantiations = 1.0f;

    [SerializeField]
    private bool m_isInfinite;

    [SerializeField]
    private int m_instantiationCount = -1;

    [SerializeField]
    private GameObject m_prefab;

    [SerializeField]
    [Tooltip("How many prefabs will be pooled on awake")]
    private int m_numberPooledPrefabs;

    #endregion

    #region Private Fields

    private GameObjectPool m_gameObjectPool;
    private WaitForSeconds delayBetweenInstantiations;

    #endregion

    private void Awake()
    {
        delayBetweenInstantiations = new WaitForSeconds(m_timeBetweenInstantiations);
        m_gameObjectPool = GetComponent<GameObjectPool>();
        m_gameObjectPool.Add(m_prefab, m_numberPooledPrefabs);
    }

    private void OnEnable()
    {
        StartCoroutine(InstantiateObjects());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator InstantiateObjects()
    {
        while(m_isInfinite || m_instantiationCount-- > 0)
        {
            Debug.Log("spawn Object");
            GameObject instance = m_gameObjectPool.Get(m_prefab);
            instance.transform.position = transform.position;
            instance.transform.rotation = Quaternion.identity;
            instance.SetActive(true);
            yield return delayBetweenInstantiations;
        }
    }
}
