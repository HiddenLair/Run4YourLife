using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;

public class AbsoluteKillRunnerOnPhisicInteraction : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner))
        {
            SendKillEvent(collider.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Runner))
        {
            SendKillEvent(collision.gameObject);
        }
    }

    void SendKillEvent(GameObject runner)
    {
        ExecuteEvents.Execute<ICharacterEvents>(runner, null, (x, y) => x.AbsoluteKill());
    }
}
