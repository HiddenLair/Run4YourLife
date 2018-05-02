using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour {

    private ParticleSystem[] particles;

	// Use this for initialization
	void Start () {
        particles = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        foreach (ParticleSystem p in particles) {
            if (p.IsAlive())
            {
                return;
            }
        }
        gameObject.SetActive(false);
    }
}
