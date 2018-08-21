using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolRequest
{
    public GameObject prefab;
    public int amount;

    //[Tooltip("Will pooled instances be shared with other objects")]
    //Will pooled instances be shared with other objects
    //private bool isShared;
}

public class GameObjectPool : MonoBehaviour
{

    [SerializeField]
    private Transform m_parent;

    private Dictionary<GameObject, List<GameObject>> m_pool = new Dictionary<GameObject, List<GameObject>>();

    public void Request(PoolRequest poolPetition)
    {
        Request(poolPetition.prefab, poolPetition.amount);
    }

    public void Request(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Add(prefab);
        }
    }

    public GameObject Add(GameObject prefab)
    {
        //Instantiate
        GameObject instance = Instantiate(prefab, m_parent);
        instance.SetActive(false);

        //Add instance to collection of pooled objects
        List<GameObject> instances;
        m_pool.TryGetValue(prefab, out instances);
        if (instances == null)
        {
            instances = new List<GameObject>();
            m_pool.Add(prefab, instances);
        }
        instances.Add(instance);

        //Add child pooled objects of instance
        PooledGameObjectDependencies poolableGameObject = prefab.GetComponent<PooledGameObjectDependencies>();
        if (poolableGameObject != null)
        {
            foreach (PoolRequest poolPetition in poolableGameObject.PoolRequests)
            {
                Request(poolPetition);
            }
        }

        return instance;
    }

    public GameObject Get(GameObject prefab)
    {
        List<GameObject> instances;
        m_pool.TryGetValue(prefab, out instances);
        if (instances != null)
        {
            foreach (GameObject g in instances)
            {
                if (!g.activeInHierarchy)
                {
                    Debug.Assert(g.transform.parent == m_parent, g);
                    return g;
                }
            }
        }

        return Add(prefab);
    }

    public GameObject GetAndPosition(GameObject key, Vector3 position, Quaternion rotation, bool activate = false)
    {
        GameObject instance = Get(key);
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.SetActive(activate);
        return instance;
    }

    public GameObject GetAndPositionAndScale(GameObject key, Vector3 position, Quaternion rotation, Vector3 scale, bool activate = false)
    {
        GameObject instance = Get(key);
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.transform.localScale = scale;
        instance.SetActive(activate);
        return instance;
    }

    public void GetActiveNonAlloc(GameObject prefab, ref List<GameObject> activeInstances)
    {
        activeInstances.Clear();
        List<GameObject> instances;
        m_pool.TryGetValue(prefab, out instances);
        if (instances != null)
        {
            foreach (GameObject g in instances)
            {
                if (g.activeInHierarchy)
                {
                    activeInstances.Add(g);
                }
            }
        }
    }
}
