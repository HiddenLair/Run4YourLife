using System.Collections;
using System.Linq;

using UnityEngine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Interactables
{
    public class DisableByBossProximity : BossDestructedInstance {

        [SerializeField]
        private float m_alphaAnimationLenght = 0.5f;

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
                StartCoroutine(AlphaAnimation(m_alphaAnimationLenght, renderer.material));
            }
        }

        public override void OnRegenerate()
        {
            StopAllCoroutines();
            for (int i = 0; i < m_renderer.Length; i++)
            {
                m_renderer[i].material = m_sharedMaterials[i];
            }
            gameObject.SetActive(true);
        }

        private IEnumerator AlphaAnimation(float trasitionLenght, Material mat)
        {
            float time = trasitionLenght;
            Color color = mat.color;
            while (time >= 0)
            {
                color.a = time / trasitionLenght;
                mat.color = color;
                yield return null;
                time -= Time.deltaTime;
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
