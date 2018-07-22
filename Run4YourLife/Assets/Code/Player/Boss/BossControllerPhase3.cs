using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement;
using Run4YourLife.CameraUtils;
using Run4YourLife.Player.Boss;

namespace Run4YourLife.Player
{
    public class BossControllerPhase3 : BossController
    {
        [SerializeField]
        private float m_fireLaserNormalizedTime;

        [SerializeField]
        private GameObject m_laserPrefab;

        protected override void ExecuteShoot() //TODO: Move strings to BossAnimation
        {
            base.ExecuteShoot();
            IsHeadLocked = true;

            m_animator.SetTrigger("Shoot");
            StartCoroutine(AnimationCallbacks.AfterStateAtNormalizedTime(m_animator, BossAnimation.StateNames.Move, m_fireLaserNormalizedTime, () => ExecuteShootCallback()));
        }

        private void ExecuteShootCallback()
        {
            AudioManager.Instance.PlaySFX(m_shotClip);

            Vector3 director = (m_crossHairControl.Position - m_shotSpawn.position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, director);
            DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_laserPrefab, m_shotSpawn.position, rotation, true);
            StartCoroutine(AnimationCallbacks.OnState(m_animator, BossAnimation.StateNames.Shoot, () => IsHeadLocked = false));
        }

        protected override void ExecuteMelee()
        {
            base.ExecuteMelee();

            m_animator.SetTrigger(BossAnimation.Triggers.Melee);
            AudioManager.Instance.PlaySFX(m_meleeClip);
        }
    }
}