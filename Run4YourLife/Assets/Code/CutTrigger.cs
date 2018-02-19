using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class CutTrigger : MonoBehaviour {

    CutPlane cuts;
    public Material crossMat;
    public float cutTime = 1.5f;
    private float timer = 0.0f;
    private GameObject g=null;

    private void Awake()
    {
        cuts = GetComponentInChildren<CutPlane>();
    }

    void Update()
    {if (g != null)
        {
            if (timer >= cutTime)
            {
                SlicedHull hull = cuts.SliceObject(g);

                if (hull != null)
                {
                    GameObject g1 = hull.CreateLowerHull(g, crossMat);
                    GameObject g2 = hull.CreateUpperHull(g, crossMat);
                    g1.AddComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
                    //g2.AddComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

                    g.SetActive(false);
                    g = g2;
                }
                timer = 0.0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        g = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        g = null;
    }
}
