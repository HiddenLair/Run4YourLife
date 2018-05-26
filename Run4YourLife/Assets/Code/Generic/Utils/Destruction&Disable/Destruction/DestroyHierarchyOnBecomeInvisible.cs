using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHierarchyOnBecomeInvisible : MonoBehaviour {

    [SerializeField]
    private int hierarchyUpLevels = 1;

    void OnBecameInvisible()
    {
        Transform t = transform;
        for(int i = 0;i< hierarchyUpLevels; ++i)
        {
            t = t.parent;
        }
        Destroy(t.gameObject);
    }
}
