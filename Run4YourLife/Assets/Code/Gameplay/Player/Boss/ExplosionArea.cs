using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;

public class ExplosionArea : MonoBehaviour {

    [SerializeField]
    private float m_explosionRadius;

    void Update () {
        Collider[] collisions = Physics.OverlapSphere(transform.position, m_explosionRadius, Layers.Runner);
        foreach (Collider c in collisions)
        {
            ExecuteEvents.Execute<ICharacterEvents>(c.gameObject, null, (x, y) => x.Kill());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_explosionRadius);
    }
}
