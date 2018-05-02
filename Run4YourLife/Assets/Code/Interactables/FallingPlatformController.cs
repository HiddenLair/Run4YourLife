using UnityEngine;
using Run4YourLife;
using Run4YourLife.Utils;

public class FallingPlatformController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody;

    [SerializeField]
    private GameObject m_FallingCollider;

    [SerializeField]
    public float m_delay;

    private bool activatedFlag = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!activatedFlag && collision.gameObject.CompareTag(Tags.Runner))
        {
            StartCoroutine(YieldHelper.WaitForSeconds(Fall, m_delay));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activatedFlag && other.CompareTag(Tags.Runner))
        {
            StartCoroutine(YieldHelper.WaitForSeconds(Fall, m_delay));
        }
    }

    private void Fall()
    {
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
        m_FallingCollider.SetActive(true);
        activatedFlag = true;
    }
}
