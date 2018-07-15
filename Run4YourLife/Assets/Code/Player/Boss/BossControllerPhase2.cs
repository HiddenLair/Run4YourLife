using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement;
using Run4YourLife.CameraUtils;

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

            GameObject bulletInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_bulletPrefab, m_shotSpawn.position, m_bulletPrefab.transform.rotation, true);
            
            Vector3 bulletDirection = (m_crossHairControl.Position - m_shotSpawn.position).normalized;
            bulletInstance.GetComponent<Rigidbody>().velocity = bulletDirection * m_bulletSpeed * Time.deltaTime;
            bulletInstance.GetComponent<ChargedBullet>().SetZValue(m_crossHairControl.Position.z);
        }
        #endregion

        #region Melee
                

        [SerializeField]
        private GameObject m_handPrefab;

        [SerializeField]
        private float m_armSpeed;

        [SerializeField]
        private GameObject m_leftHandGameObject;

        [SerializeField]
        private GameObject m_rightHandGameObject;

        [SerializeField]
        private float m_meleeNormalizedTime;

        private Vector3 m_meleePosition;
        private Quaternion m_meleeRotation;
        private bool m_isExecutingMeleeRight;

        protected override void ExecuteMelee()
        {
            base.ExecuteMelee();

            AudioManager.Instance.PlaySFX(m_meleeClip);


            m_meleePosition = m_crossHairControl.Position;
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 screenPos = mainCamera.WorldToViewportPoint(m_meleePosition);

            if (screenPos.x <= 0.5f)
            {
                m_animator.SetTrigger("MeleR");
                m_meleePosition.x = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1,1)).x;
                m_meleeRotation = Quaternion.identity;
                m_isExecutingMeleeRight = true;

                StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, "MeleRight", m_meleeNormalizedTime, () => ExecuteMeleeCallback()));
            }
            else
            {
                m_animator.SetTrigger("MeleL");
                m_meleePosition.x = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0,0)).x;
                m_meleeRotation = Quaternion.Euler(0, 180, 0);
                m_isExecutingMeleeRight = false;

                StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, "MeleLeft", m_meleeNormalizedTime, () => ExecuteMeleeCallback()));
            }
        }

        private void ExecuteMeleeCallback()
        {
            GameObject handInstance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_handPrefab, m_meleePosition, m_meleeRotation, true);
            handInstance.GetComponent<Rigidbody>().velocity = handInstance.transform.right * m_armSpeed * Time.deltaTime;

            if (m_isExecutingMeleeRight)
            {
                m_rightHandGameObject.SetActive(false);
            }
            else
            {
                m_leftHandGameObject.SetActive(false);
            }
        }

        protected override void OnShootReady() 
        {
            if (m_isExecutingMeleeRight)
            {
                m_rightHandGameObject.SetActive(true);
            }
            else
            {
                m_leftHandGameObject.SetActive(true);
            }
        }

        #endregion
    }
}