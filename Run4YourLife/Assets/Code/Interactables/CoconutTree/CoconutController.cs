using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Interactables
{

    [RequireComponent(typeof(Rigidbody))]
    public class CoconutController : MonoBehaviour
    {

        [SerializeField]
        private AudioClip m_releaseCoconutAudioClip;

        [SerializeField]
        private AudioClip m_hitGroundAudioClip;

        [SerializeField]
        private FXReceiver m_coconutExplosionParticles;


        public bool IsIdle { get; private set; }

        private Rigidbody m_rigidbody;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            Reset();
        }

        public void Fall()
        {
            IsIdle = false;
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
            AudioManager.Instance.PlaySFX(m_releaseCoconutAudioClip);
        }

        public void Reset()
        {
            IsIdle = true;
            gameObject.SetActive(true);
            m_rigidbody.useGravity = false;
            m_rigidbody.isKinematic = true;
            transform.localPosition = Vector3.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            IRunnerEvents runnerEvents;
            if (!IsIdle && ValidTarget(other, out runnerEvents))
            {
                transform.position = ExtractTopObjectPosition(other);
                Explode();

                if (runnerEvents != null)
                {
                    runnerEvents.Kill();
                }
            }
        }

        public bool ValidTarget(Collider other, out IRunnerEvents runnerEvents)
        {
            runnerEvents = other.GetComponent<IRunnerEvents>();
            return Layers.Stage.Contains(other.gameObject.layer) || runnerEvents != null;
        }

        public Vector3 ExtractTopObjectPosition(Collider other)
        {
            Vector3 topObjectPosition = transform.position;
            topObjectPosition.y = other.bounds.center.y + other.bounds.extents.y;
            return topObjectPosition;
        }

        private void Explode()
        {
            AudioManager.Instance.PlaySFX(m_hitGroundAudioClip);
            m_coconutExplosionParticles.PlayFx(false);
            gameObject.SetActive(false);
        }
    }
}