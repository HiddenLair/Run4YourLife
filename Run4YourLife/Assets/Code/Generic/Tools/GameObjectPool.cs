using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

    private Dictionary<GameObject, List<GameObject>> m_pool = new Dictionary<GameObject, List<GameObject>>();


    public void Add(GameObject key, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Add(key);
        }
    }

    public GameObject Add(GameObject key)
    {
        GameObject instance = Instantiate(key, transform);
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
}
