using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class ParticleFadeOut : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private ParticleSystemRenderer particleSystemRenderer;

        [SerializeField]
        public float timeToFade = 2.0f;

        #endregion

        #region Variables

        private bool Faded = false;

        #endregion

        public void RestoreParticleMaterial()
        {
            Color originalColor = particleSystemRenderer.material.GetColor("_TintColor");
            particleSystemRenderer.material.SetColor("_TintColor", new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f));
        }

        public void RestoreParticleTrailMaterial()
        {
            Color originalColor = particleSystemRenderer.trailMaterial.GetColor("_TintColor");
            particleSystemRenderer.trailMaterial.SetColor("_TintColor", new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f));
        }

        public void FadeOutParticleMaterial()
        {
            StartCoroutine(FadeMaterial());
        }

        public void FadeOutParticleTrailMaterial()
        {
            StartCoroutine(FadeTrail());
        }

        IEnumerator FadeTrail()
        {
            float timePassed = 0.0f;

            Faded = false;

            while (!Faded)
            {
                timePassed += Time.deltaTime;
                float percent = timePassed / timeToFade;

                Color originalColor = particleSystemRenderer.trailMaterial.GetColor("_TintColor");

                originalColor.a = 1.0f - percent;

                particleSystemRenderer.trailMaterial.SetColor("_TintColor", new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a));

                if (originalColor.a < 0.001f)
                {
                    Faded = true;
                }
                else
                {
                    yield return null;
                }
            }
        }

        IEnumerator FadeMaterial()
        {
            float timePassed = 0.0f;

            while (!Faded)
            {
                timePassed += Time.deltaTime;
                float percent = timePassed / timeToFade;

                Color originalColor = particleSystemRenderer.material.GetColor("_TintColor");

                originalColor.a = 1.0f - percent;

                particleSystemRenderer.material.SetColor("_TintColor", new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a));

                if (originalColor.a < 0.001f)
                {
                    Faded = true;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}
