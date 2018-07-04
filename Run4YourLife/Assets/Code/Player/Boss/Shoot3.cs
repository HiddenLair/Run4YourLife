using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
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

        [SerializeField]
        private float distanceToFollow;

        [SerializeField]
        [Range(-90,90)]
        private float rotationReceiverOffset;

        #endregion

        private float timeToShootFromAnim = 0.2f;

        private void Start()
        {
            Vector3 newScale = dangerIndicator.transform.localScale;
            newScale.x = newScale.z = laserWidth;
            newScale.y = maxLaserDistance;
            dangerIndicator.transform.localScale = newScale;
            Vector3 newLocalPos = dangerIndicator.transform.localPosition;
            newLocalPos.x += newScale.y;
            dangerIndicator.transform.localPosition = newLocalPos;

        }

        public override void ShootByAnim()
        {
            SetUpLoadReceiver();

            AudioManager.Instance.PlaySFX(m_shotClip);
            loadShootReceiver.PlayFx(true);
            m_animator.SetTrigger("Shoot");
            StartCoroutine(AnimationCallbacks.AfterStateAtNormalizedTime(m_animator, "ChargeShoot", timeToShootFromAnim, () => Shoot()));
            StartCoroutine(AnimationCallbacks.AfterStateAtNormalizedTime(m_animator, "ChargeShoot", 0.0f, () => StopIndicator()));
            dangerIndicator.SetActive(true);
        }

        void StopIndicator()
        {
            dangerIndicator.SetActive(false);
        }

        void Shoot()
        {
            Vector3 director = (m_crossHairControl.Position - loadShootReceiver.transform.position).normalized;

            RaycastHit[] targetLocation;
            GameObject particle = FXManager.Instance.InstantiateFromValues(loadShootReceiver.transform.position, Quaternion.identity, loadedShootParticles,loadShootReceiver.transform);
            particle.transform.localRotation = loadedShootParticles.transform.rotation;

            targetLocation = Physics.SphereCastAll(loadShootReceiver.transform.position, laserWidth, director, maxLaserDistance, Layers.Runner | Layers.Trap);
            foreach (RaycastHit r in targetLocation)
            {
                ExecuteEvents.Execute<IRunnerEvents>(r.transform.gameObject, null, (x, y) => x.Kill());
            }
        }

        void SetUpLoadReceiver()
        {
            Vector3 rot = Quaternion.Euler(0.0f, 0.0f, rotationReceiverOffset) * head.right;

            loadShootReceiver.transform.position = head.position + rot.normalized * distanceToFollow;

            Vector3 director = (m_crossHairControl.Position - loadShootReceiver.transform.position).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, director);

            loadShootReceiver.transform.localRotation = rotation;
        }
    }
}