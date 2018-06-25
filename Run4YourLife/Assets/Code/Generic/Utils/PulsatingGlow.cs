using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingGlow : MonoBehaviour
{
    [SerializeField]
    private float maxIntensity = 3;

    [SerializeField]
    private float pulseSpeed = 1;

    private Renderer m_renderer;

    void Start ()
    {
        m_renderer = GetComponentInChildren<Renderer>();
	}

	void Update () 
    {
        m_renderer.material.SetFloat("_EmissionForce", Mathf.PingPong(Time.time * pulseSpeed, maxIntensity));
	}
}
