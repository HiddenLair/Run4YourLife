using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedBullet : MonoBehaviour {

    public GameObject explosion;

    private Boss callback;

    private void OnTriggerEnter(Collider other)
    {
        Explosion();
        //TODO: destroy if it goes out off screen 
    }

    public void Explosion()
    {
        Instantiate(explosion,transform.position,transform.rotation);
        callback.SetShootStillAlive(false);
        Destroy(gameObject);
    }

    public void SetCallback(Boss boss)
    {
        callback = boss;
    }
}
