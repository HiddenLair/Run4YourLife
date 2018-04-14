using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CononutLauncher : MonoBehaviour {

    #region Inspector

    [SerializeField]
    private float shootForce;

    [SerializeField]
    private float timeBetweenShoots;

    [SerializeField]
    private Transform shootInitZone;

    [SerializeField]
    private GameObject bullet;

    #endregion

    #region Variables

    private float timer=0.0f;

    #endregion

    private void Awake()
    {
        timer = Time.time + timeBetweenShoots;
    }

    // Update is called once per frame
    void Update () {
		if(timer <= Time.time)
        {
            Shoot();
            timer = Time.time + timeBetweenShoots;
        }
	}

    private void Shoot()
    {
        GameObject g = Instantiate(bullet,shootInitZone.position,transform.rotation*bullet.transform.rotation, transform);
        g.GetComponent<Rigidbody>().AddForce(g.transform.up * shootForce);
    }
}
