using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;

[RequireComponent(typeof(Rigidbody))]
public class CoconutController : MonoBehaviour
{

    [SerializeField]
    private AudioClip m_releaseCoconutAudioClip;

    [SerializeField]
    private AudioClip m_hitGroundAudioClip;


    public bool Aviable { get; private set; }

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        Reset();
    }

    public void Fall()
    {
        Aviable = false;
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
        AudioManager.Instance.PlaySFX(m_releaseCoconutAudioClip);
    }

    public void Reset()
    {
        Aviable = true;
        gameObject.SetActive(true);
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Aviable) // it is falling
        {
            IRunnerEvents runnerEvents = other.gameObject.GetComponent<IRunnerEvents>();
            if (runnerEvents != null)
            {
                runnerEvents.Kill();
            }
            else
            {
                AudioManager.Instance.PlaySFX(m_hitGroundAudioClip);
            }

            gameObject.SetActive(false);

        }
    }
}
