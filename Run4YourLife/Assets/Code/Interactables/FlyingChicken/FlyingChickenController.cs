using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables.FlyingChicken
{
    public class FlyingChickenController : MonoBehaviour, IRunnerDashBreakable
    {

        [SerializeField]
        private Vector3 m_velocity;

        [SerializeField]
        private FXReceiver m_explodeParticles;

        private Rigidbody m_rigidbody;

        private RunnerDashBreakableState m_state;

        RunnerDashBreakableState IRunnerDashBreakable.RunnerDashBreakableState { get { return m_state; } }

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void ResetState()
        {
            m_state = RunnerDashBreakableState.Alive;
        }

        private void FixedUpdate()
        {
            m_rigidbody.MovePosition(transform.position + m_velocity * Time.deltaTime);
        }

        public void OnCollidedWithRunner(RunnerController runnerController)
        {
            if (!runnerController.IsDashing)
            {
                runnerController.Kill();
            }
        }

        void IRunnerDashBreakable.Break()
        {
            m_state = RunnerDashBreakableState.Broken;

            m_explodeParticles.PlayFx(false);
            gameObject.SetActive(false);
        }
    }
}