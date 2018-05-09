using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Interactables;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(RunnerControlScheme))]
    public class RunnerGoThroughPlatformsController : MonoBehaviour
    {
        [SerializeField]
        [Range(-1f,1f)]
        private float m_inputThreshold = -0.9f;

        private InputController m_inputController;
        private RunnerControlScheme m_runnerControlScheme;

        private void Awake()
        {
            m_inputController = GetComponent<InputController>();
            m_runnerControlScheme = GetComponent<RunnerControlScheme>();
        }

        private void Update()
        {
            if (m_inputController.Value(m_runnerControlScheme.Vertical) < m_inputThreshold)
            {
                PlatformGoThroughManager.Instance.IgnoreCollision(gameObject);
            }
        }
    }
}
