using System.Collections;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class TrapBase : MonoBehaviour
    {
        [SerializeField]
        private float m_fadeInTime;

        [SerializeField]
        private float m_cooldown;

        public float Cooldown { get { return m_cooldown; } }

        private Collider m_collider;
        private Renderer m_renderer;
        private Rigidbody m_rigidbody;

        protected virtual void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_collider = GetComponent<Collider>();
            m_renderer = GetComponentInChildren<Renderer>();
            Debug.Assert(m_renderer != null);

            m_collider.enabled = false;

            Color actualC = m_renderer.material.color;
            actualC.a = 0;
            m_renderer.material.color = actualC;
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(FadeInAndFall());
        }

        private IEnumerator FadeInAndFall()
        {
            yield return StartCoroutine(FadeIn());
            yield return StartCoroutine(Fall());

            m_collider.enabled = true;
            m_rigidbody.useGravity = false;
            m_rigidbody.isKinematic = true;
        }

        private IEnumerator FadeIn()
        {
            float startTime = Time.time;
            Color color = m_renderer.material.color;
            while (color.a < 1)
            {
                color.a = Mathf.Min(Time.time - startTime, m_fadeInTime) / m_fadeInTime;
                m_renderer.material.color = color;
                yield return null;
            }

            MakeOpaque(m_renderer.material);
        }

        private IEnumerator Fall()
        {
            m_rigidbody.useGravity = true;

            while (true)
            {
                RaycastHit info;
                if (Physics.Raycast(transform.position, Vector3.down, out info, 0.5f, Layers.Stage, QueryTriggerInteraction.Ignore))
                {
                    transform.position = transform.position + Vector3.down * info.distance;
                    transform.SetParent(info.collider.gameObject.transform); //Set ground as parent

                    break;
                }
                yield return null;
            }
        }

        private void MakeOpaque(Material material)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }
    }
}