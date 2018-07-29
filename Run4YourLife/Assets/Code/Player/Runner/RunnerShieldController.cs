using System;
using System.Collections;
using System.Collections.Generic;
using Run4YourLife.Utils;
using UnityEngine;

namespace Run4YourLife.Player.Runner
{
    [RequireComponent(typeof(MeshRenderer))]
    public class RunnerShieldController : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1)]
        private float m_atenuation;

        private MeshRenderer m_meshRenderer;
        private Material m_shieldMaterial;
        private Coroutine m_shieldBehaviour;
        private PlayerInstance m_playerInstance;

        public void Init(PlayerInstance playerInstance)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            m_shieldMaterial = m_meshRenderer.material;
            m_playerInstance = playerInstance;
        }

        private Color ColorOfCharacterType(CharacterType characterType)
        {
            Color color = Color.black;

            switch (characterType)
            {
                case CharacterType.ACorn:
                    color = Color.blue;
                    break;
                case CharacterType.Skull:
                    color = Color.magenta;
                    break;
                case CharacterType.Snake:
                    color = Color.yellow;
                    break;
                case CharacterType.Plain:
                    color = Color.red;
                    break;
                case CharacterType.NoColor:
                    color = Color.gray;
                    break;
            }

            return color;
        }

        public void ActivateShield(float duration)
        {
            if (m_shieldBehaviour != null)
            {
                StopCoroutine(m_shieldBehaviour);
            }

            m_shieldBehaviour = StartCoroutine(ShieldBehaviour(duration));
        }

        public void DeactivateShield()
        {
            if (m_shieldBehaviour != null)
            {
                StopCoroutine(m_shieldBehaviour);
                m_shieldBehaviour = null;
            }

            m_meshRenderer.enabled = false;
        }

        private IEnumerator ShieldBehaviour(float duration)
        {
            m_shieldMaterial.color = ColorOfCharacterType(m_playerInstance.PlayerHandle.CharacterType);
            m_meshRenderer.enabled = true;

            float endTime = Time.time + duration;
            while (Time.time < endTime)
            {
                float percentage = ((endTime - Time.time) / duration);
                Color color = m_shieldMaterial.color;
                color.a = percentage * m_atenuation;
                m_shieldMaterial.color = color;
                yield return null;
            }

            m_meshRenderer.enabled = false;
        }
    }
}
