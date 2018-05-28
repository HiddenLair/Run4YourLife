using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;

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
            AnimationPlayOnTimeManager.Instance.PlayOnAnimation(animator, "Shoot", timeToShootFromAnim, () => Shoot());        
        }

        void Shoot()
        {
            AudioManager.Instance.PlayFX(AudioManager.Sfx.BossShoot);

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