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
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Tremble();
            Destroy(gameObject, timeToDestroy);
        }
    }

    private void Tremble()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.Runner);
        foreach(GameObject player in players)
        {
            player.GetComponent<RunnerCharacterController>().AddVelocity(new Vector3(0, playerPushForce, 0));
        }
        //Camera.main.GetComponent<CameraBossFollow>().AddTrauma(cameraTraumaShake);
    }
}
