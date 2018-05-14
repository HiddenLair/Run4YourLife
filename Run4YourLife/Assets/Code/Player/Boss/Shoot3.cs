using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Shoot3 : Shoot
    {
        #region Inspector

        [SerializeField]
        private GameObject laser;

        [SerializeField]
        private GameObject dangerIndicator;

        [SerializeField]
        private float maxLaserDistance = 50.0f;

        [SerializeField]
        private float laserWidth = 1.0f;

        #endregion

        private float timeToShootFromAnim = 0.2f;

        private void Start()
        {
            Vector3 newScale = dangerIndicator.transform.localScale;
            newScale.x = newScale.z = laserWidth;
            newScale.y = maxLaserDistance;
            dangerIndicator.transform.localScale = newScale;
            Vector3 newPos = new Vector3(newScale.y, 0, 0);
            dangerIndicator.transform.localPosition = newPos;
        }

        public override void ShootByAnim()
        {
            animator.SetTrigger("Shoot");
            AnimationPlayOnTimeManager.Instance.PlayOnNextAnimation(animator, "ChargeShoot", timeToShootFromAnim, () => Shoot());
            AnimationPlayOnTimeManager.Instance.PlayOnNextAnimation(animator, "ChargeShoot", 0.0f, () => StopIndicator());
            dangerIndicator.SetActive(true);
        }

        void StopIndicator()
        {
            dangerIndicator.SetActive(false);
        }

        void Shoot()
        {
            RaycastHit[] targetLocation;

            laser.SetActive(true);
            targetLocation = Physics.SphereCastAll(shootInitZone.position, laserWidth, shootInitZone.right, maxLaserDistance, Layers.Runner | Layers.Trap);
            foreach (RaycastHit r in targetLocation)
            {
                ExecuteEvents.Execute<ICharacterEvents>(r.transform.gameObject, null, (x, y) => x.Kill());
            }
        }
    }
}