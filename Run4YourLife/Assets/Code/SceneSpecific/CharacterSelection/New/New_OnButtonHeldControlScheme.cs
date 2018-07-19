using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.InputManagement
{
    public class New_OnButtonHeldControlScheme : OnButtonPressedControlScheme
    {
        // IMPROVE: Prevent collision between different input sources

        [SerializeField]
        private UnityEvent onButtonHeld;

        [SerializeField]
        private UnityEvent onCompleted;

        [SerializeField]
        private float timeS;

        [SerializeField]
        private float percentIgnoreOnHeld;

        private bool started = false;
        private bool completed = false;
        private float initialTimeS = Mathf.Infinity;

        public float GetPercent()
        {
            return started ? Mathf.Clamp01(ComputeCurrentTimeS() / timeS) : 0.0f;
        }

        protected override void Update()
        {
            base.Update();

            if(InputAction.Persists())
            {
                OnHeld();
            }
        }

        void LateUpdate()
        {
            if(completed)
            {
                OnCompleted();
            }
        }

        protected override void OnPressed()
        {
            started = true;
            initialTimeS = Time.time;

            base.OnPressed();
        }

        protected override void OnReleased()
        {
            started = false;
            completed = false;
            initialTimeS = Mathf.Infinity;

            base.OnReleased();
        }

        private void OnHeld()
        {
            completed = ComputeCurrentTimeS() >= timeS;

            if(GetPercent() >= percentIgnoreOnHeld)
            {
                onButtonHeld.Invoke();
            }
        }

        private void OnCompleted()
        {
            started = false;
            completed = false;
            initialTimeS = Mathf.Infinity;

            onCompleted.Invoke();
        }

        private float ComputeCurrentTimeS()
        {
            return Time.time - initialTimeS;
        }
    }
}