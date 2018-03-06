using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {

    class InnerPos
    {
        public int listOffset;
        public float progressionOffset;
    }

    [SerializeField]
    private List<CheckPoint> list;
    private Dictionary<int, InnerPos> idMap = new Dictionary<int, InnerPos>();
    static private int idSetter = 0;

    private static CheckPointManager instance;

    public static CheckPointManager Instance()
    {
        if (instance == null)
        {
            instance = (CheckPointManager)FindObjectOfType(typeof(CheckPointManager));

            if (FindObjectsOfType(typeof(CheckPointManager)).Length > 1)
            {
                Debug.LogError("[Singleton] Something went really wrong " +
                    " - there should never be more than 1 singleton!" +
                    " Reopenning the scene might fix it.");
                return instance;
            }

            if (instance == null)
            {
                GameObject singleton = new GameObject();
                instance = singleton.AddComponent<CheckPointManager>();
                singleton.name = "(singleton) " + typeof(CheckPointManager).ToString();

                DontDestroyOnLoad(singleton);

                Debug.Log("[Singleton] An instance of " + typeof(CheckPointManager) +
                    " is needed in the scene, so '" + singleton +
                    "' was created with DontDestroyOnLoad.");
            }
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public int Subscribe()
    {
        int ret = idSetter;
        idSetter++;
        idMap[ret] = new InnerPos();
        return ret;
    }

    public Vector3 GetPosition( int id, float speed)
    {
        Vector3 ret = Vector3.zero;
        if (idMap.ContainsKey(id))
        {
            InnerPos inner = idMap[id];
            if(inner.listOffset == list.Count-2 && inner.progressionOffset >= 1)
            {
                return list[inner.listOffset+1].transform.position; //We stand in the last point
            }

            Vector3 point1 = list[inner.listOffset].transform.position;
            Vector3 point2 = list[inner.listOffset+1].transform.position;
            float lerpSpeed = speed / Vector3.Distance(point1, point2);
            inner.progressionOffset += lerpSpeed;
            if (inner.progressionOffset > 1 && inner.listOffset < list.Count-2)
            {
                inner.listOffset++;
                float percent = (inner.progressionOffset - 1)/lerpSpeed;
                Vector3 point3 = list[inner.listOffset + 1].transform.position;
                lerpSpeed = speed / Vector3.Distance(point2, point3);
                inner.progressionOffset = lerpSpeed*percent;
            }
            return Vector3.Lerp(point1,point2,Mathf.Clamp01(inner.progressionOffset));
        }
        Debug.LogError("Invalid Id");
        return ret;
    }

}
