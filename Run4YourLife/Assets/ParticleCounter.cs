using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Debugging
{
    public class ParticleCounter : DebugFeature
    {
        protected override string GetPanelName()
        {
            return "Particle Counter";
        }

        protected override void OnCustomDrawGUI()
        {
            int particleCount = 0;
            ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
            foreach (ParticleSystem p in particleSystems)
            {
                particleCount += p.particleCount;
            }

            GUILayout.Label(particleCount.ToString());
        }
    }
}
