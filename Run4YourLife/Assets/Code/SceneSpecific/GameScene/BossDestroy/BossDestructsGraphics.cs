using System.Collections;
using System.Linq;

using UnityEngine;

using Run4YourLife.GameManagement;
using UnityEngine.Events;

namespace Run4YourLife.Interactables
{
    public class BossDestructsGraphics : BossDestructedInstance {

        [SerializeField]
        private float m_alphaAnimationLenght = 0.5f;

        [SerializeField]
        private bool m_activateOnRegenerate = true;

        [SerializeField]
        private UnityEvent m_onRegenerated;

        private Renderer[] m_renderer;
        private Material[] m_sharedMaterials;

        protected override void Awake()
        {
            base.Awake();
            m_renderer = GetComponentsInChildren<Renderer>();
            m_sharedMaterials = m_renderer.Select((x) => x.sharedMaterial).ToArray();
        }

        public override void OnBossDestroy()
        {
            IsDestructed = true;
            if(gameObject.activeInHierarchy)
            {
                foreach (Renderer renderer in m_renderer)
                {
                    SetInitialTiling(renderer.material);
                }

                StartCoroutine(AlphaAnimation());
            }            
        }

        public override void OnRegenerate()
        {
            IsDestructed = false;
            ResetRenderer();

            m_onRegenerated.Invoke();

            gameObject.SetActive(m_activateOnRegenerate);
        }

        private void OnDisable()
        {
            ResetRenderer();
        }

        private void ResetRenderer()
        {
            StopAllCoroutines();
            for (int i = 0; i < m_renderer.Length; i++)
            {
                m_renderer[i].material = m_sharedMaterials[i];
            }
        }

        private IEnumerator AlphaAnimation()
        {            
            float endTime = Time.time + m_alphaAnimationLenght;
            float startTime = Time.time;
            while(Time.time < endTime)
            {
                foreach (Renderer renderer in m_renderer)
                {
                    if (renderer.material.HasProperty("_Dissolveamout"))
                    {
                        float dissolve = renderer.material.GetFloat("_Dissolveamout");
                        dissolve = 1 - (endTime - Time.time) / m_alphaAnimationLenght;
                        renderer.material.SetFloat("_Dissolveamout", dissolve);
                    }
                }   
                yield return null;
            }

            gameObject.SetActive(false);
        }

        private void SetInitialTiling(Material mat)
        {
            if (mat.HasProperty("_Noise"))
            {
                float x = Mathf.Sin(Time.time);
                float y = Mathf.Cos(Time.time);
                mat.SetTextureOffset("_Noise", new Vector2(x, y));
            }
        }
    }
}
