using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool destroyOnTime = false;

    [SerializeField]
    private float destroyTime = 1.0f;

    #endregion

    #region Private Fields

    private WaitForSeconds delayBetweenInstantiations;

    #endregion

    private void Awake()
    {
        delayBetweenInstantiations = new WaitForSeconds(m_timeBetweenInstantiations);
        StartCoroutine(InstantiateObjects());
    }

    private IEnumerator InstantiateObjects()
    {
        while(m_isInfinite || m_instantiationCount-- > 0)
        {
            yield return delayBetweenInstantiations;
            GameObject temp = Instantiate(m_prefab, transform.position, transform.rotation, transform);
            if (destroyOnTime)
            {
                Destroy(temp, destroyTime);
            }
        }
    }
}
