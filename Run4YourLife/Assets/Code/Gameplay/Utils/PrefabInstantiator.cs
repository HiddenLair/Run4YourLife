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
            Instantiate(m_prefab, transform.position, transform.rotation, transform);
        }
    }
}
