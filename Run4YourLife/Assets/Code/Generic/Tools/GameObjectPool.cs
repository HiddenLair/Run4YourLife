using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

    [SerializeField]
    private Transform m_parent;

    [SerializeField]
    private float m_timeBetweenRetrivals = 10;

    private float m_nextRetrivalTime;

    private Dictionary<GameObject, List<GameObject>> m_pool = new Dictionary<GameObject, List<GameObject>>();


    private void OnEnable()
    {
        m_nextRetrivalTime = Time.time + m_timeBetweenRetrivals;
    }

    public void Add(GameObject key, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Add(key);
        }
    }

    public GameObject Add(GameObject key)
    {
        GameObject instance = Instantiate(key, m_parent);
        instance.SetActive(false);

        List<GameObject> instances;
        m_pool.TryGetValue(key, out instances);
        if(instances == null)
        {
            instances = new List<GameObject>();
            m_pool.Add(key, instances);
        }
        instances.Add(instance);
        return instance;
    }

    public GameObject Get(GameObject key)
    {
        List<GameObject> instances;
        m_pool.TryGetValue(key, out instances);
        if(instances != null)
        {
            foreach(GameObject g in instances)
            {
                if(!g.activeInHierarchy && (g.transform.parent == m_parent))
                {
                    return g;
                }
            }
        }

        return Add(key);
    }

    public GameObject GetAndPosition(GameObject key, Vector3 position, Quaternion rotation, bool activate = false)
    {
        GameObject instance = Get(key);
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.SetActive(activate);
        return instance;
    }

    private void Update()
    {
        if(m_nextRetrivalTime <= Time.time)
        {
            RetrieveObjectsToPool();
            m_nextRetrivalTime = Time.time + m_timeBetweenRetrivals;
        }
    }

    private void RetrieveObjectsToPool()
    {
        foreach (KeyValuePair<GameObject, List<GameObject>> item in m_pool)
        {
            foreach(GameObject g in item.Value)
            {
                if (!g.activeInHierarchy)
                {
                    g.transform.SetParent(m_parent);
                    g.transform.localPosition = Vector3.zero;
                    g.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }
}
