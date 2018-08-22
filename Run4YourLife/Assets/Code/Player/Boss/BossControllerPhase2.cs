using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement;
using Run4YourLife.CameraUtils;
using Run4YourLife.Player.Boss;

namespace Run4YourLife.Player.Boss
{
    public class BossControllerPhase2 : BossController
    {

        #region  Shoot

        [SerializeField]
        private GameObject m_bulletPrefab;

        [SerializeField]
        private float m_bulletSpeed;

        [SerializeField]
        private float m_shootBulletNormalizedTime;

        protected override void ExecuteShoot()
        {
            base.ExecuteShoot();
            m_animator.SetTrigger(BossAnimation.Triggers.Shoot);

            IsHeadLocked = true;

            AudioManager.Instance.PlaySFX(m_shotWarningClip);

            Vector3 position = m_crossHairControl.Position;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, (position - m_shotSpawn.position).normalized);

            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, BossAnimation.Triggers.Shoot, m_shootBulletNormalizedTime, () => ExecuteShootCallback(position, rotation)));
        }

        private void ExecuteShootCallback(Vector3 position, Quaternion rotation)
        {
            GameObject bulletInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_bulletPrefab, m_shotSpawn.position, rotation, true);
            bulletInstance.GetComponent<BossExplosiveBulletController>().LaunchBullet(position);

            AudioManager.Instance.PlaySFX(m_shotFireClip);

            IsHeadLocked = false;
        }
        #endregion

        #region Melee


        [SerializeField]
        private GameObject m_leftHandPrefab;

        [SerializeField]
        private GameObject m_rightHandPrefab;

        [SerializeField]
        private float m_armSpeed;

        [SerializeField]
        private GameObject m_leftHandGraphics;

        [SerializeField]
        private GameObject m_rightHandGraphics;

        [SerializeField]
        private float m_meleeNormalizedTime;

        private bool m_isExecutingMeleeRight;

        protected override void ExecuteMelee()
        {
            base.ExecuteMelee();

            AudioManager.Instance.PlaySFX(m_meleeClip);

            bool rightHand;
            Vector3 position = m_crossHairControl.Position;
            string animation;

            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 screenPos = mainCamera.WorldToViewportPoint(position);
            if (screenPos.x <= 0.5f)
            {
                m_animator.SetTrigger(BossAnimation.Triggers.MeleR); // Boss is flipped so it executes the inverted animation
                position.x = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0)).x;
                rightHand = true;
                animation = BossAnimation.StateNames.MeleRight;
            }
            else
            {
                m_animator.SetTrigger(BossAnimation.Triggers.MeleL); // Boss is flipped so it executes the inverted animation
                position.x = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1)).x;
                rightHand = false;
                animation = BossAnimation.StateNames.MeleLeft;
            }

            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, animation, m_meleeNormalizedTime, () => ExecuteMeleeCallback(position, rightHand)));
        }

        private void ExecuteMeleeCallback(Vector3 position, bool rightHand)
        {
            if (rightHand)
            {
                m_rightHandGraphics.SetActive(false);
                GameObject handInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_rightHandPrefab, position, Quaternion.identity, true);
                handInstance.GetComponent<Rigidbody>().velocity = handInstance.transform.right * m_armSpeed * Time.deltaTime;
            }
            else
            {
                m_leftHandGraphics.SetActive(false);
                GameObject handInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_leftHandPrefab, position, Quaternion.identity, true);
                handInstance.GetComponent<Rigidbody>().velocity = -handInstance.transform.right * m_armSpeed * Time.deltaTime;
            }

            m_isExecutingMeleeRight = rightHand;
        }

        protected override void OnShootReady()
        {
            if (m_isExecutingMeleeRight)
            {
                m_rightHandGraphics.SetActive(true);
            }
            else
            {
                m_leftHandGraphics.SetActive(true);
            }
        }

        #endregion
    }
}