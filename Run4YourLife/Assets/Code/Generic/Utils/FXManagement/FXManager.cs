using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SingletonMonoBehaviour<FXManager> {

    public Transform fxDefaultParent;
    Dictionary<GameObject, List<GameObject>> fxPool = new Dictionary<GameObject, List<GameObject>>();

    public GameObject InstantiateFromReceiver(FXReceiver receiver,GameObject prefab,bool setAsParent = false)
    {
        if (!fxPool.ContainsKey(prefab))
        {
            fxPool.Add(prefab,new List<GameObject>());
        }

        Transform parent = fxDefaultParent;
        if (setAsParent)
        {
            parent = receiver.transform.parent;
        }

        List<GameObject> tempList = fxPool[prefab];
        for(int i=0; i<tempList.Count; ++i)
        {
            GameObject tempG = tempList[i];
            if (!tempG.activeInHierarchy)
            {
                tempG.transform.position = receiver.transform.position;
                tempG.transform.rotation = receiver.transform.rotation;
                tempG.transform.SetParent(parent);
                tempG.SetActive(true);
                return tempG;
            }
        }
        GameObject newG = Instantiate(prefab,receiver.transform.position,receiver.transform.rotation, parent);
        tempList.Add(newG);
        return newG;
    }

    public GameObject InstantiateFromValues(Vector3 position,Quaternion rotation, GameObject prefab, Transform parent=null)
    {
        if (!fxPool.ContainsKey(prefab))
        {
            fxPool.Add(prefab, new List<GameObject>());
        }

        if(parent == null)
        {
            parent = fxDefaultParent;
        }


        List<GameObject> tempList = fxPool[prefab];
        for (int i = 0; i < tempList.Count; ++i)
        {
            GameObject tempG = tempList[i];
            if (!tempG.activeInHierarchy)
            {
                tempG.transform.position = position;
                tempG.transform.rotation = rotation;
                tempG.transform.SetParent(parent);
                tempG.SetActive(true);
                return tempG;
            }
        }
        GameObject newG = Instantiate(prefab, position, rotation,parent);
        tempList.Add(newG);
        return newG;
    }
}
