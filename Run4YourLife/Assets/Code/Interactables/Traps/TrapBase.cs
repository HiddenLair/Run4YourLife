using System.Collections;
using UnityEngine;
using Run4YourLife.Utils;

namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(Collider))]
    public abstract class TrapBase : MonoBehaviour
    {
        [SerializeField]
        private float m_generationAnimationLength = 0.5f;

        [SerializeField]
        private float m_fadeInTime;

        [SerializeField]
        private float m_cooldown;

        [SerializeField]
        private float rayCheckerLenght = 10.0f;

        [SerializeField]
        private float gravity = -9.8f;

        [SerializeField]
        private float initialSpeed = 0;

        public float Cooldown { get { return m_cooldown; } }

        private Collider m_collider;
        private Renderer m_renderer;
        private Vector3 finalPos;
        private Vector3 speed = Vector3.zero;
        private bool destroyOnLanding = false;
        private Transform fatherTransformStorage;
        private float fatherInitialY;

        protected virtual void Awake()
        {
            m_collider = GetComponent<Collider>();
            m_renderer = GetComponentInChildren<Renderer>();
            Debug.Assert(m_renderer != null);
            m_collider.enabled = false;

            speed.y = initialSpeed;

            SetInitialTiling(m_renderer.material);
        }

        protected virtual void OnEnable()
        {
            speed.y = initialSpeed;

            RaycastHit info;
            if (Physics.Raycast(transform.position, Vector3.down, out info, rayCheckerLenght, Layers.Stage))
            {
                if (info.collider.CompareTag(Tags.Water))
                {
                    destroyOnLanding = true;
                }
                finalPos = transform.position + Vector3.down * info.distance;
                fatherTransformStorage = info.collider.gameObject.transform;
                fatherInitialY = fatherTransformStorage.position.y;
            }

            StartCoroutine(FadeInAndFall());
        }

        private void OnDisable()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator FadeInAndFall()
        {
            yield return StartCoroutine(GenerateTrap());
            yield return StartCoroutine(Fall());

            m_collider.enabled = true;
        }

        private IEnumerator GenerateTrap()
        {
            float endTime = Time.time + m_fadeInTime;
            float startTime = Time.time;
            while (Time.time < endTime)
            {
                if(m_renderer.material.HasProperty("_Dissolveamout"))
                {
                    float dissolve = m_renderer.material.GetFloat("_Dissolveamout");
                    dissolve = (endTime - Time.time) / m_fadeInTime;
                    m_renderer.material.SetFloat("_Dissolveamout", dissolve);
                }                
                yield return null;
            }
        }

        private IEnumerator Fall()
        {
            while(true)
            {
                transform.Translate(speed * Time.deltaTime);
                float yVar = fatherTransformStorage.position.y - fatherInitialY;
                if (transform.position.y < finalPos.y + yVar)
                {
                    finalPos.y += yVar;
                    transform.position = finalPos;
                    transform.SetParent(fatherTransformStorage);
                    if (destroyOnLanding)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                }
                speed.y += gravity * Time.deltaTime;
                yield return null;
            }
        }

        private void SetInitialTiling(Material mat)
        {
            if(mat.HasProperty("_Noise"))
            {
                float x = Mathf.Sin(Time.time);
                float y = Mathf.Cos(Time.time);
                mat.SetTextureOffset("_Noise", new Vector2(x, y));
            }          
        }
    }
}