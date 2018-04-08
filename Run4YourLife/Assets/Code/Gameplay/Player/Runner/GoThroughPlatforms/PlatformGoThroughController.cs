using UnityEngine;

using Run4YourLife.GameManagement;

public class PlatformGoThroughController : MonoBehaviour {

    private PlatformGoThroughManager m_platformGoThroughManager;
    private Collider m_collider;

    private void Awake()
    {
        enabled = false;

        m_platformGoThroughManager = FindObjectOfType<PlatformGoThroughManager>();
        Debug.Assert(m_platformGoThroughManager != null);

        m_collider = GetComponent<Collider>();
        Debug.Assert(m_collider != null);
    }

    private void LateUpdate()
    {
        foreach (GameObject runner in GameObject.FindGameObjectsWithTag(Tags.Runner))
        {
            Collider collider = runner.GetComponent<Collider>();
            if (!m_platformGoThroughManager.IgnoredRunners.Contains(runner))
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
