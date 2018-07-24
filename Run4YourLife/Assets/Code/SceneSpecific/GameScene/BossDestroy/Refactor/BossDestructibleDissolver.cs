using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Run4YourLife.GameManagement.Refactor
{
    public class BossDestructibleDissolver : BossDestructibleBase
    {

        [SerializeField]
        private float m_disolveAnimationDuration;

        private Renderer[] m_renderer;
        private Material[] m_sharedMaterials;

        protected override void Awake()
        {
            base.Awake();

            m_renderer = GetComponentsInChildren<Renderer>();
            m_sharedMaterials = m_renderer.Select((x) => x.sharedMaterial).ToArray();
        }

        protected override void DestroyBehaviour()
        {
            foreach (Renderer renderer in m_renderer)
            {
                SetRandomTilingTimeBased(renderer.material);
            }

            StartCoroutine(DisolveAndDisable());
        }

        protected override void RegenerateBehaviour()
        {
            ResetState();

            gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ResetState();
        }

        private void ResetState()
        {
            StopAllCoroutines();
            for (int i = 0; i < m_renderer.Length; i++)
            {
                m_renderer[i].material = m_sharedMaterials[i];
            }
        }

        private IEnumerator DisolveAndDisable()
        {
            float endTime = Time.time + m_disolveAnimationDuration;
            float startTime = Time.time;
            while (Time.time < endTime)
            {
                foreach (Renderer renderer in m_renderer)
                {
                    if (renderer.material.HasProperty("_Dissolveamout"))
                    {
                        float dissolve = renderer.material.GetFloat("_Dissolveamout");
                        dissolve = 1 - (endTime - Time.time) / m_disolveAnimationDuration;
                        renderer.material.SetFloat("_Dissolveamout", dissolve);
                    }
                }
                yield return null;
            }

            gameObject.SetActive(false);
        }

        private void SetRandomTilingTimeBased(Material material)
        {
            if (material.HasProperty("_Noise"))
            {
                material.SetTextureOffset("_Noise", new Vector2()
                {
                    x = Mathf.Sin(Time.time),
                    y = Mathf.Cos(Time.time)
                });
            }
        }
    }
}