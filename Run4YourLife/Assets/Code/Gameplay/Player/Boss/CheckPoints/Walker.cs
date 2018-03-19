using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Walker : MonoBehaviour {


    public float speed;
    public bool increaseSpeedOverTime;
    public float increaseValue;
    private int id;
    private CheckPointManager checkPointManager;

    void Start()
    {
        checkPointManager = FindObjectsOfType<CheckPointManager>().Where(x => x.enabled).First();
        id = checkPointManager.Subscribe();
    }

    void Update()
    {
        if (increaseSpeedOverTime)
        {
            speed += increaseValue * Time.deltaTime;
        }
        checkPointManager.Compute(id, speed);

        transform.position = checkPointManager.GetPosition(id,speed);

        float fH;
        Vector3 pO;

        checkPointManager.GetFloorHeightAndPositionOffset(id, speed, out fH, out pO);

        CameraBossFollow cameraBossFollow = Camera.main.GetComponent<CameraBossFollow>();

        cameraBossFollow.bossAndFloorHeight = fH;
        cameraBossFollow.bossPositionOffset = pO;
    }
}
