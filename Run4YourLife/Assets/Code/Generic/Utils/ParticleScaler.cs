using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour {

    List<Transform> particlesToScale = new List<Transform>();

	// Use this for initialization
	void Start () {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            particlesToScale.Add(p.transform);
        }
	}
	
	public void SetScale(Vector3 scale)
    {
        foreach (Transform transform in particlesToScale)
        {
            transform.localScale = scale;
        }
    }

    public void AddToScale(Vector3 variation)
    {
        foreach (Transform transform in particlesToScale)
        {
            transform.localScale += variation;
        }
    }
}
