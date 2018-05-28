using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.InputManagement;
using UnityEngine.EventSystems;
using Run4YourLife.UI;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Ready))]
    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CrossHairControl))]
    public abstract class Shoot : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        protected Transform head;

        [SerializeField]
        protected Transform shootInitZone;

        [SerializeField]
        [Range(0, 1)]
        private float triggerSensivility = 0.2f;

        [SerializeField]
        private float reloadTimeS;

        [SerializeField]
        [Range(-90, 90)]
        private float m_lookAtOffset;

        [SerializeField]
        protected AudioClip sfx;

        #endregion

        private float currentTimeS = 0;
        private bool shootTrigger = false;
        protected Quaternion initialRotation;

        private Ready ready;
        private BossControlScheme controlScheme;

        protected CrossHairControl crossHairControl;
        protected Animator animator;
        protected AudioSource audioSource;

        private GameObject uiManager;

        private void Awake()
        {
            ready = GetComponent<Ready>();
            controlScheme = GetComponent<BossControlScheme>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            crossHairControl = GetComponent<CrossHairControl>();
            uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(uiManager != null, "UI manager gameobject not found");

            currentTimeS = Time.time + reloadTimeS;
            initialRotation = head.rotation;
        }

        private void Start()
        {
            controlScheme.Active = true;
        }

        private void Update()
        {
            FireBullet();
        }

        private void LateUpdate()
        {
            RotateHead();
        }

        public virtual void RotateHead()
        {
            head.LookAt(crossHairControl.Position);
            head.Rotate(0, -90, m_lookAtOffset);
            head.rotation *= initialRotation;
        }

        private void FireBullet()
        {
            if (ready.Get())
            {
                if (controlScheme.Shoot.Value() > triggerSensivility)
                {
                    if (currentTimeS <= Time.time && !shootTrigger)
                    {
                        ShootByAnim();
                        currentTimeS = Time.time + reloadTimeS;
                        shootTrigger = true;
                        crossHairControl.Lock();

                        ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.SHOOT, reloadTimeS));
                    }
                }
                else
                {
                    shootTrigger = false;
                    crossHairControl.Unlock();
                }
            }
        }

        public abstract void ShootByAnim();

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(head.position, crossHairControl.Position);
        }
    }
}
