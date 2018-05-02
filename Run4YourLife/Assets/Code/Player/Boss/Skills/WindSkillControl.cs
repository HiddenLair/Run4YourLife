using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerCharacterController))]
    public class WindSkillControl : SkillBase
    {

        [SerializeField]
        private float timeToDie = 5;

        [SerializeField]
        private float windForce;

        private void OnEnable()
        {
            Destroy(gameObject, timeToDie);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                Wind wind = collider.gameObject.GetComponent<Wind>();
                if (wind == null)
                {
                    wind = collider.gameObject.AddComponent<Wind>();
                }

                wind.EnterWindArea(this);
            }
        }

        public float GetWindForce()
        {
            return windForce;
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                Wind wind = collider.gameObject.GetComponent<Wind>();
                wind.ExitWindArea(this);
            }
        }
    }
}