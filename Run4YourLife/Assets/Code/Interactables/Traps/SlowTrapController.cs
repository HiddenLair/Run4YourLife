﻿using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;

using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class SlowTrapController : TrapBase
    {
        [SerializeField]
        private GameObject m_activationParticles;

        [SerializeField]
        private StatusEffectSet m_statusEffectSet;

        [SerializeField]
        private float m_slowTime;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                StatusEffectController statusEffectController = collider.GetComponent<StatusEffectController>();
                statusEffectController.AddAndRemoveAfterTime(m_statusEffectSet, m_slowTime);

                AudioManager.Instance.PlaySFX(m_trapDetonationClip);
                Instantiate(m_activationParticles, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }
    }
}