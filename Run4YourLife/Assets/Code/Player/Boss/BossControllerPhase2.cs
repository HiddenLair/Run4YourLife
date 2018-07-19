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
            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, BossAnimation.Triggers.Shoot, m_shootBulletNormalizedTime, () => ExecuteShootCallback()));
        }

        private void ExecuteShootCallback()
        {
            AudioManager.Instance.PlaySFX(m_shotClip);

            Vector3 director = (m_crossHairControl.Position - m_shotSpawn.position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, director);
            GameObject bulletInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_bulletPrefab, m_shotSpawn.position, rotation, true);
            bulletInstance.GetComponent<BossExplosiveBulletController>().LaunchBullet(m_crossHairControl.Position);
        }
        #endregion

        #region Melee
                

        [SerializeField]
        private GameObject m_handPrefab;

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
            Quaternion rotation;
            Vector3 position = m_crossHairControl.Position;
            string animation;

            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 screenPos = mainCamera.WorldToViewportPoint(position);
            if (screenPos.x <= 0.5f)
            {
                m_animator.SetTrigger("MeleR"); // Boss is flipped so it executes the inverted animation
                position.x = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0,0)).x;
                rotation = Quaternion.identity;
                rightHand = true;
                animation = "MeleRight";
            }
            else
            {
                m_animator.SetTrigger("MeleL"); // Boss is flipped so it executes the inverted animation
                position.x = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1,1)).x;
                rotation = Quaternion.Euler(0, 180, 0);
                rightHand = false;
                animation = "MeleLeft";
            }

            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, animation, m_meleeNormalizedTime, () => ExecuteMeleeCallback(position, rotation, rightHand)));
        }

        private void ExecuteMeleeCallback(Vector3 position, Quaternion rotation, bool rightHand)
        {
            GameObject handInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_handPrefab, position, rotation, true);
            handInstance.GetComponent<Rigidbody>().velocity = handInstance.transform.right * m_armSpeed * Time.deltaTime;

            if (rightHand)
            {
                m_rightHandGraphics.SetActive(false);
            }
            else
            {
                m_leftHandGraphics.SetActive(false);
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