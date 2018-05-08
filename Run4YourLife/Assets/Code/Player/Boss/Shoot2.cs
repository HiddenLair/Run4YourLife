using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
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
            audioSource.PlayOneShot(sfx);

            Vector3 director = (crossHairControl.GetPosition() - shootInitZone.position).normalized;
            GameObject tempInstance = Instantiate(instance, shootInitZone.position, instance.transform.rotation);
            tempInstance.GetComponent<Rigidbody>().velocity = director * bulletSpeed * Time.deltaTime;
            tempInstance.GetComponent<ChargedBullet>().SetZValue(crossHairControl.GetPosition().z);
        }

        public override void RotateHead()
        {
            head.LookAt(crossHairControl.GetPosition());
            head.Rotate(0, -180, 0);
            head.rotation *= initialRotation;
        }
    }
}