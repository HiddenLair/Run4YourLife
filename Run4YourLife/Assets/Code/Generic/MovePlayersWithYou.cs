using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayersWithYou : MonoBehaviour {

    public Vector3 speed;

    List<Transform> players = new List<Transform>();
	
	// Update is called once per frame
	void Update () {
        foreach (Transform transform in players)
        {
            transform.Translate(speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        players.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        players.Remove(other.transform);
    }
}
