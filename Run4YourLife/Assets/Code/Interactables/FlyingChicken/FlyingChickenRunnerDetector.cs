using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables.FlyingChicken
{
    public class FlyingChickenRunnerDetector : MonoBehaviour
    {
        [SerializeField]
        private FlyingChickenController m_flyingChickenController;

        private void OnTriggerEnter(Collider other)
        {
            RunnerController runnerController = other.GetComponent<RunnerController>();
            if (runnerController != null)
            {
                m_flyingChickenController.OnCollidedWithRunner(runnerController);
            }
        }
    }
}
