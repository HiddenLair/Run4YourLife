using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Destructible")
        {
            Destroy(collision.gameObject);
        }
        if (collision.collider.tag == "Destructible" || collision.collider.tag == "Ground")
        {
            Destroy(gameObject);
        }         
    }

}
