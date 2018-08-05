using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class KillRunnerOnPhysicInteraction : MonoBehaviour
    {

        private void OnTriggerEnter(Collider collider)
        {
            CheckRunnerAndSendKillEvent(collider.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckRunnerAndSendKillEvent(collision.gameObject);
        }

        void CheckRunnerAndSendKillEvent(GameObject runner)
        {
            IRunnerEvents runnerEvents = runner.GetComponent<IRunnerEvents>();
            if (runnerEvents != null)
            {
                runnerEvents.Kill();
            }
        }
    }
}