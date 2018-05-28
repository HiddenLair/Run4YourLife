using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Shoot1 : Shoot
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
            Vector3 cPos = crossHairControl.Position;
            cPos.z = 0;
            Vector3 director = (crossHairControl.Position - shootInitZone.position).normalized;
            Quaternion rot = Quaternion.FromToRotation(Vector3.right,director);
            GameObject tempInstance = Instantiate(instance, shootInitZone.position, instance.transform.rotation * rot);
            tempInstance.GetComponent<Rigidbody>().velocity = director * bulletSpeed * Time.deltaTime;
        }
    }
}
