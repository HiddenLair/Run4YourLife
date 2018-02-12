using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBullet : MonoBehaviour {

    public float force;
    public float maxForce;

    private GameObject callBack;
    private Rigidbody body;

	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float yInput = Input.GetAxis("RightJoystickY1");
        if (Mathf.Abs(yInput) > 0.2)
        {
            if (yInput < 0)
            {
                body.AddForce(new Vector3(0,force,0));
            }
            else
            {
                body.AddForce(new Vector3(0, -force, 0));
            }

        }
        float xInput = Input.GetAxis("RightJoystickX1");
        if (Mathf.Abs(xInput) > 0.2)
        {
            if (xInput < 0)
            {
                body.AddForce(new Vector3(force, 0, 0));
            }
            else
            {
                body.AddForce(new Vector3(-force, 0, 0));
            }

        }

        if(body.velocity.x > maxForce)
        {
            body.velocity = new Vector3(maxForce,body.velocity.y,0);
        }

        if (body.velocity.y > maxForce)
        {
            body.velocity = new Vector3(body.velocity.x, maxForce, 0);
        }

    }

    public void SetCallback(GameObject gameObject)
    {
        callBack = gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Destructible")
        {
            Destroy(collision.gameObject);
        }
        if (collision.collider.tag == "Destructible" || collision.collider.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        callBack.GetComponent<Boss>().SetShootStillAlive();
    }
}
