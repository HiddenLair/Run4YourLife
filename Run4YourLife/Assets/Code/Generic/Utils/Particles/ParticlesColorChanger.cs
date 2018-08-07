using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class ParticlesColorChanger : MonoBehaviour
    {


        struct ParticleStruct
        {
            public ParticleSystem particle;
            public Color color;
            public ParticleStruct(ParticleSystem particle, Color color)
            {
                this.particle = particle;
                this.color = color;
            }
        }

        private List<ParticleStruct> particles = new List<ParticleStruct>();

        // Use this for initialization
        void Start()
        {
            ParticleSystem[] temp = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in temp)
            {
                particles.Add(new ParticleStruct(p, p.main.startColor.color));
            }
        }

        public void ChangeColor(Color color, float time)
        {
            StartCoroutine(ChangeColorInTime(color, time));
        }

        IEnumerator ChangeColorInTime(Color color, float time)
        {
            List<ParticleStruct> temp = new List<ParticleStruct>(particles);
            float timer = Time.time + time;
            while (timer > Time.time)
            {
                for (int i = 0; i < particles.Count; ++i)
                {
                    ParticleStruct p = particles[i];
                    p.color = Color.Lerp(temp[i].color, color, 1 - (timer - Time.time / time));
                    ParticleSystem.MainModule pMain = p.particle.main;
                    pMain.startColor = p.color;
                }
                yield return null;
            }
        }
    }
}
