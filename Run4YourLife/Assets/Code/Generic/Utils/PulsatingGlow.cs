using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingGlow : MonoBehaviour
{
    [SerializeField]
    private float maxIntensity = 3;

    [SerializeField]
    private float pulseSpeed = 1;

    private Renderer[] m_renderers;

    void Start ()
    {
        m_renderers = GetComponentsInChildren<Renderer>();
	}

	void Update () 
    {
        foreach (Renderer renderer in m_renderers)
        {
            renderer.material.SetFloat("_EmissionForce", Mathf.PingPong(Time.time * pulseSpeed, maxIntensity));
        }
	}
}
