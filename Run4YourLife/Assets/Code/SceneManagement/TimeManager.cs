using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.SceneManagement;
using Run4YourLife.GameManagement;

public class TimeManager : MonoBehaviour {

    public float timeInPhase;

    public GameObject sceneLoadRequests;

    private void Start()
    {
        GetComponent<PlayerSpawner>().InstantiateBossPlayer();
        GetComponent<PlayerSpawner>().InstantiatePlayers(); 
    }

    // Update is called once per frame
    void Update () {
        timeInPhase -= Time.deltaTime;
        if(timeInPhase <= 0)
        {
            foreach (SceneLoadRequest request in sceneLoadRequests.GetComponents<SceneLoadRequest>())
            {
                request.Execute();
            }
        }
	}
}
