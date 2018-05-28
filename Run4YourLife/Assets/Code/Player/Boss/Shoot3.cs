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
        private GameObject dangerIndicator;

        [SerializeField]
        private Transform dangerRotator;

        [SerializeField]
        private float maxLaserDistance = 50.0f;

        [SerializeField]
        private float laserWidth = 1.0f;

        [SerializeField]
        private FXReceiver loadShootReceiver;

        [SerializeField]
        private GameObject loadedShootParticles;

        #endregion

        private float timeToShootFromAnim = 0.2f;

        private void Start()
        {
            Vector3 newScale = dangerIndicator.transform.localScale;
            newScale.x = newScale.z = laserWidth;
            newScale.y = maxLaserDistance;
            dangerIndicator.transform.localScale = newScale;
            dangerIndicator.transform.position = loadShootReceiver.transform.position;
            Vector3 newLocalPos = dangerIndicator.transform.localPosition;
            newLocalPos.x += newScale.y;
            dangerIndicator.transform.localPosition = newLocalPos;

        }

        public override void ShootByAnim()
        {
            Vector3 director = (crossHairControl.Position - loadShootReceiver.transform.position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, director);

            dangerRotator.localRotation = rotation;

            loadShootReceiver.PlayFx();
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
            Vector3 director = (crossHairControl.Position - loadShootReceiver.transform.position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right,director);

            RaycastHit[] targetLocation;
            FXManager.Instance.InstantiateFromValues(loadShootReceiver.transform.position, rotation, loadedShootParticles,loadShootReceiver.transform);

            targetLocation = Physics.SphereCastAll(loadShootReceiver.transform.position, laserWidth, director, maxLaserDistance, Layers.Runner | Layers.Trap);
            foreach (RaycastHit r in targetLocation)
            {
                ExecuteEvents.Execute<ICharacterEvents>(r.transform.gameObject, null, (x, y) => x.Kill());
            }
        }
    }
}