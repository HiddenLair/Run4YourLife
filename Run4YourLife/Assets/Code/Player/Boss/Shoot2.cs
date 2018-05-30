using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    public class Shoot2 : Shoot
    {
        #region Inspector

        [SerializeField]
        private GameObject instance;

        [SerializeField]
        private float bulletSpeed;

        #endregion

        private float timeToShootFromAnim = 0.5f;

        public override void ShootByAnim()
        {
            animator.SetTrigger("Shoot");
            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(animator, "Shoot", timeToShootFromAnim, () => Shoot()));
        }

        void Shoot()
        {
            AudioManager.Instance.PlaySFX(m_shotClip);

            Vector3 director = (crossHairControl.Position - shootInitZone.position).normalized;
            GameObject tempInstance = Instantiate(instance, shootInitZone.position, instance.transform.rotation);
            tempInstance.GetComponent<Rigidbody>().velocity = director * bulletSpeed * Time.deltaTime;
            tempInstance.GetComponent<ChargedBullet>().SetZValue(crossHairControl.Position.z);
        }

        public override void RotateHead()
        {
            head.LookAt(crossHairControl.Position);
            head.Rotate(0, -180, 0);
            head.rotation *= initialRotation;
        }
    }
}