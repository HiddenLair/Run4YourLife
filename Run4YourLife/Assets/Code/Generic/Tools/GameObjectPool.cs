using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

    [SerializeField]
    private Transform m_parent;

    [SerializeField]
    private float updatePeriod = 10;

    private float timer;

    private Dictionary<GameObject, List<GameObject>> m_pool = new Dictionary<GameObject, List<GameObject>>();


    private void Start()
    {
        timer = Time.time + updatePeriod;
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
                if(!g.activeSelf)
                {
                    return g;
                }
            }
        }

        return Add(key);
    }

    private void Update()
    {
        if(timer <= Time.time)
        {
            RetrieveObjectsToPool();
            timer = Time.time + updatePeriod;
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
                }
            }
        }
    }

    private void Reset()
    {
        m_parent = transform;
    }
}
