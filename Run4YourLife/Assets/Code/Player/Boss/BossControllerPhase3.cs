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

        protected override void ExecuteShoot()
        {
            base.ExecuteShoot();
            IsHeadLocked = true;

            m_animator.SetTrigger(BossAnimation.Triggers.Shoot);
            StartCoroutine(AnimationCallbacks.AfterStateAtNormalizedTime(m_animator, BossAnimation.StateNames.Move, m_fireLaserNormalizedTime, () => ExecuteShootCallback()));
        }

        private void ExecuteShootCallback()
        {
            AudioManager.Instance.PlaySFX(m_shotClip);

            Vector3 director = (m_crossHairControl.Position - m_shotSpawn.position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, director);
            rotation.z = 0;
            GameObject laserInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_laserPrefab, m_shotSpawn.position, rotation, true);
            laserInstance.GetComponent<SimulateChildOf>().Parent = m_shotSpawn;


            // we wait for the Shoot state because at this moment in time, the code is in a transition moving towards shoot
            // when in a transition we are still not in the state
            // it happens to be that when the transition ends, the particle ends too
            StartCoroutine(AnimationCallbacks.OnState(m_animator, BossAnimation.StateNames.Shoot, () => IsHeadLocked = false));
        }

        protected override void ExecuteMelee()
        {
            base.ExecuteMelee();

            m_animator.SetTrigger(BossAnimation.Triggers.Mele);
            AudioManager.Instance.PlaySFX(m_meleeClip);
        }
    }
}