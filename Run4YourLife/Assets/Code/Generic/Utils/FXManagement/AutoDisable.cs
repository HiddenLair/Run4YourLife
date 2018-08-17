using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    private ParticleSystem[] m_particleSystems;

    private void Awake()
    {
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        foreach (ParticleSystem p in m_particleSystems)
        {
            if (p.IsAlive(false))
            {
                return;
            }
        }

        gameObject.SetActive(false);
    }
}
