using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;

public class RootTrapControl : MonoBehaviour
{
    #region Public variables
    public GameObject activationParticles;
    public int rootHardness = 5;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner) || collider.CompareTag(Tags.Shoot))
        {
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.Root(rootHardness));
            Instantiate(activationParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
