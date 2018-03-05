using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

public class MelePhase3 : MonoBehaviour {

    public float timeToDestroy;
    public float playerPushForce;
    public float cameraTraumaShake;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(other.gameObject);
        }

        if(other.tag == "Ground")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Tremble();
            Destroy(gameObject,timeToDestroy);
        }
    }

    private void Tremble()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject g in players)
        {
            g.GetComponent<PlayerCharacterController>().AddVelocity(new Vector3(0, playerPushForce, 0));
        }
        Camera.main.GetComponent<CameraBossFollow>().AddTrauma(cameraTraumaShake);
    }
}
