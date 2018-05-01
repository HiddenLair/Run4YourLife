using UnityEngine;
using System.Linq;

using Cinemachine;
using Run4YourLife.Cinemachine;

public class Walker : MonoBehaviour {


    public float speed;
    public bool increaseSpeedOverTime;
    public float increaseValue;
    private int id;
    private CheckPointManager checkPointManager;

    void Start()
    {
        checkPointManager = FindObjectsOfType<CheckPointManager>().Where(x => x.enabled).First(); // TODO: When walker is fixed do this better
        id = checkPointManager.Subscribe();
    }

    private void OnEnable()
    {
        if(checkPointManager != null)
        {
            id = checkPointManager.Subscribe();
        }
    }

    private void OnDisable()
    {
        if (checkPointManager != null)
        {
            checkPointManager.Unsubscribe(id);
        }
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (increaseSpeedOverTime)
            {
                speed += increaseValue * Time.deltaTime;
            }
            checkPointManager.Compute(id, speed);

            transform.position = checkPointManager.GetPosition(id, speed);

            float fH;
            Vector3 pO;

            checkPointManager.GetFloorHeightAndPositionOffset(id, speed, out fH, out pO);

            try
            {
                CinemachineScreenTransposer cameraBossFollow = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineScreenTransposer>();

                cameraBossFollow.m_verticalHeight = fH;
                cameraBossFollow.m_offsetFromTarget = pO;
            }
            catch
            {

            }
        }
    }
}
