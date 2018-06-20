using UnityEngine;

namespace Run4YourLife.Utils
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MaterialFadeOut : MonoBehaviour
    {
        private float endTimeS;
        private float durationS;
        private Material material;
        private float initialAlpha;

        private bool activated = false;

        void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
            initialAlpha = material.color.a;
        }

        void Update()
        {
            if(activated)
            {
                if(Time.time >= endTimeS)
                {
                    activated = false;
                }
                else
                {
                    UpdateAlpha((endTimeS - Time.time) / durationS);
                }
            }
        }

        public void Activate(float durationS)
        {
            ResetAlpha();

            activated = true;
            this.durationS = durationS;
            endTimeS = Time.time + durationS;
        }

        private void ResetAlpha()
        {
            UpdateAlpha(1.0f);
        }

        private void UpdateAlpha(float t)
        {
            Color color = material.color;
            color.a = t * initialAlpha;
            material.color = color;
        }
    }
}