using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {

    class InnerPos
    {
        public int listOffset;
        public float progressionOffset;
    }

    private Transform[] checkpoints;
    private Dictionary<int, InnerPos> idMap = new Dictionary<int, InnerPos>();
    static private int idSetter = 0;

    private void Awake()
    {
        checkpoints = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            checkpoints[i] = transform.GetChild(i);
        }
    }

    /*private static CheckPointManager instance;

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
    }*/

    public int Subscribe()
    {
        int ret = idSetter;
        idSetter++;
        idMap[ret] = new InnerPos();
        return ret;
    }

    public void Unsubscribe(int id)
    {
        idMap.Remove(id);
    }

    public void Compute(int id, float speed)
    {
        if(idMap.ContainsKey(id))
        {
            InnerPos inner = idMap[id];

            if(inner.listOffset == checkpoints.Length - 2 && inner.progressionOffset >= 1)
            {
                return;
            }

            Vector3 point1 = checkpoints[inner.listOffset].position;
            Vector3 point2 = checkpoints[inner.listOffset + 1].position;

            BossSlowPoint slow = checkpoints[inner.listOffset].GetComponent<BossSlowPoint>();
            if (slow) {
                speed = speed - speed * slow.percentSpeedReduction;
            }

            float lerpSpeed = speed / Vector3.Distance(point1, point2);
            inner.progressionOffset += lerpSpeed;

            if(inner.progressionOffset > 1 && inner.listOffset < checkpoints.Length - 2)
            {
                inner.listOffset++;

                float percent = (inner.progressionOffset - 1) / lerpSpeed;
                Vector3 point3 = checkpoints[inner.listOffset + 1].transform.position;
                lerpSpeed = speed / Vector3.Distance(point2, point3);
                inner.progressionOffset = lerpSpeed * percent;
            }

            return;
        }

        Debug.LogError("Invalid Id");
    }

    public Vector3 GetPosition( int id, float speed)
    {
        Vector3 ret = Vector3.zero;

        if(idMap.ContainsKey(id))
        {
            InnerPos inner = idMap[id];

            if(inner.listOffset == checkpoints.Length-2 && inner.progressionOffset >= 1)
            {
                return checkpoints[inner.listOffset+1].position; //We stand in the last point
            }

            Vector3 point1 = checkpoints[inner.listOffset].position;
            Vector3 point2 = checkpoints[inner.listOffset+1].position;
            //float lerpSpeed = speed / Vector3.Distance(point1, point2);

            /* inner.progressionOffset += lerpSpeed;
            if (inner.progressionOffset > 1 && inner.listOffset < checkpoints.Length-2)
            {
                inner.listOffset++;
                float percent = (inner.progressionOffset - 1)/lerpSpeed;
                Vector3 point3 = checkpoints[inner.listOffset + 1].transform.position;
                lerpSpeed = speed / Vector3.Distance(point2, point3);
                inner.progressionOffset = lerpSpeed*percent;
            } */

            return Vector3.Lerp(point1,point2,Mathf.Clamp01(inner.progressionOffset));
        }

        Debug.LogError("Invalid Id");

        return ret;
    }

    public void GetFloorHeightAndPositionOffset(int id, float speed, out float floorHeight, out Vector3 positionOffset)
    {
        floorHeight = 0.0f;
        positionOffset = Vector3.zero;

        if(idMap.ContainsKey(id))
        {
            InnerPos inner = idMap[id];

            if(inner.listOffset == checkpoints.Length - 2 && inner.progressionOffset >= 1)
            {
                int index = inner.listOffset + 1;

                CameraAttributesDefinition cameraAttributesDefinition = checkpoints[index].GetComponent<CameraAttributesDefinition>();

                floorHeight = cameraAttributesDefinition.desiredFloorHeight;
                positionOffset = cameraAttributesDefinition.desiredPositionOffset;

                return;
            }

            int index1 = inner.listOffset;
            int index2 = inner.listOffset + 1;

            CameraAttributesDefinition cameraAttributesDefinition1 = checkpoints[index1].GetComponent<CameraAttributesDefinition>();
            CameraAttributesDefinition cameraAttributesDefinition2 = checkpoints[index2].GetComponent<CameraAttributesDefinition>();

            //CameraBossFollow cameraBossFollow = Camera.main.GetComponent<CameraBossFollow>();

            float fH1 = cameraAttributesDefinition1.desiredFloorHeight;
            float fH2 = cameraAttributesDefinition2.desiredFloorHeight;

            Vector3 oP1 = cameraAttributesDefinition1.desiredPositionOffset;
            Vector3 oP2 = cameraAttributesDefinition2.desiredPositionOffset;

            floorHeight = Mathf.Lerp(fH1, fH2, Mathf.Clamp01(inner.progressionOffset));
            positionOffset = Vector3.Lerp(oP1, oP2, Mathf.Clamp01(inner.progressionOffset));

            return;
        }

        Debug.LogError("Invalid Id");
    }
}