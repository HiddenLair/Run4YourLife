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

        private Color originalColor;

        #endregion

        private void Awake()
        {
            originalColor = particleSystemRenderer.trailMaterial.GetColor("_TintColor");
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

                float newAlphaValue = originalColor.a;

                newAlphaValue = 1.0f - percent;

                particleSystemRenderer.trailMaterial.SetColor("_TintColor", new Color(originalColor.r, originalColor.g, originalColor.b, newAlphaValue));

                if (newAlphaValue < 0.001f)
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

                Color originalMatColor = particleSystemRenderer.material.GetColor("_TintColor");

                originalMatColor.a = 1.0f - percent;

                particleSystemRenderer.material.SetColor("_TintColor", new Color(originalMatColor.r, originalMatColor.g, originalMatColor.b, originalMatColor.a));

                if (originalMatColor.a < 0.001f)
                {
                    Faded = true;
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void OnDisable()
        {
            particleSystemRenderer.trailMaterial.SetColor("_TintColor", new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a));
        }
    }
}
