using UnityEngine;

namespace Run4YourLife.UI
{
    public abstract class Updater : MonoBehaviour
    {
        private float time = 0.0f;

        private float currentTime = 0.0f;

        private bool disabled = false;

        void Update()
        {
            if(time > 0.0f)
            {
                if((currentTime += Time.deltaTime) >= time)
                {
                    time = 0.0f;
                    currentTime = 0.0f;
                }
                else
                {
                    float remainingTime = time - currentTime;
                    float remainingTimePercent = remainingTime / time;

                    OnCoolDown(remainingTime, remainingTimePercent);
                }
            }
            else
            {
                if(disabled)
                {
                    OnDisabled();
                }
                else
                {
                    OnEnabled();
                }
            }
        }

        public void Use(float time)
        {
            this.time = time;
            currentTime = 0.0f;
        }

        public void Enable(bool enabled)
        {
            disabled = !enabled;
        }

        protected abstract void OnEnabled();

        protected abstract void OnDisabled();

        protected abstract void OnCoolDown(float remainingTime, float remainingTimePercent);
    }
}