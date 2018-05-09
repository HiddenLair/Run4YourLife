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

        private void Awake()
        {
            m_renderer = GetComponentsInChildren<Renderer>();
            m_sharedMaterials = m_renderer.Select((x) => x.sharedMaterial).ToArray();
        }

        public override void OnBossDestroy()
        {
            foreach (Renderer renderer in m_renderer)
            {
                MakeTransparent(renderer.material);
            }

            StartCoroutine(AlphaAnimation());
        }

        public override void OnRegenerate()
        {
            StopAllCoroutines();
            for (int i = 0; i < m_renderer.Length; i++)
            {
                m_renderer[i].material = m_sharedMaterials[i];
            }

            m_onRegenerated.Invoke();

            gameObject.SetActive(m_activateOnRegenerate);
        }

        private IEnumerator AlphaAnimation()
        {            
            float endTime = Time.time + m_alphaAnimationLenght;
            float startTime = Time.time;
            while(Time.time < endTime)
            {
                foreach (Renderer renderer in m_renderer)
                {
                    Color color = renderer.material.color;
                    color.a = (endTime - Time.time) / m_alphaAnimationLenght;
                    renderer.material.color = color;
                }   
                yield return null;
            }

            gameObject.SetActive(false);
        }

        public void MakeTransparent(Material material)
        {
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
}
