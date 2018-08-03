using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour {

    List<Transform> particlesToScale = new List<Transform>();

	// Use this for initialization
	void Awake () {
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

    public void SetXScale(float x)
    {
        foreach (Transform transform in particlesToScale)
        {
            Vector3 temp = transform.localScale;
            temp.x = x;
            transform.localScale = temp;
        }
    }

    public void SetYScale(float y)
    {
        foreach (Transform transform in particlesToScale)
        {
            Vector3 temp = transform.localScale;
            temp.y = y;
            transform.localScale = temp;
        }
    }

    public void SetZScale(float z)
    {
        foreach (Transform transform in particlesToScale)
        {
            Vector3 temp = transform.localScale;
            temp.z = z;
            transform.localScale = temp;
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
