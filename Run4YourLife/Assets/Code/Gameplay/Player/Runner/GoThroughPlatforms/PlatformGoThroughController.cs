using UnityEngine;

using Run4YourLife;
using Run4YourLife.GameManagement;

public class PlatformGoThroughController : MonoBehaviour {

    private Collider m_collider;

    private void Awake()
    {
        enabled = false;

        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null);
    }

    private void LateUpdate()
    {
        foreach (GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
        {
            Collider collider = runner.GetComponent<Collider>();
            if (!PlatformGoThroughManager.Instance.IgnoredRunners.Contains(runner))
            {
                bool isRunnerOnBottom = runner.transform.position.y < transform.position.y;
                Physics.IgnoreCollision(m_collider, collider, isRunnerOnBottom);
            }
            else
            {
                Physics.IgnoreCollision(m_collider, collider);
            }
        }
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }
}
