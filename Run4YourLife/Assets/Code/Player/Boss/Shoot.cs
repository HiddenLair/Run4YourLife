using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.InputManagement;
using Run4YourLife.UI;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CrossHairControl))]
    public abstract class Shoot : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float reloadTimeS;
        
        [SerializeField]
        protected Transform shootInitZone;

        [SerializeField]
        protected AudioClip m_shotClip;

        [SerializeField]
        [Range(0, 1)]
        private float triggerSensivility = 0.2f;

        [SerializeField]
        [Tooltip("The transform for the head's bone used to drive the head")]
        protected Transform head;

        [SerializeField]
        [Range(-90, 90)]
        [Tooltip("Offset from wich the head will look, used to shoot with the mouth instead of the beak")]
        private float m_lookAtOffset;

        #endregion


        private float m_readyToShootTime;

        private GameObject m_ui;
        
        protected Quaternion m_initialHeadRotation;

        private BossControlScheme m_controlScheme;
        protected CrossHairControl m_crossHairControl;
        protected Animator m_animator;

        private void Awake()
        {
            m_controlScheme = GetComponent<BossControlScheme>();
            m_animator = GetComponent<Animator>();
            m_crossHairControl = GetComponent<CrossHairControl>();
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);

            m_readyToShootTime = Time.time + reloadTimeS;
            m_initialHeadRotation = head.rotation;
        }

        private void Start()
        {
            m_controlScheme.Active = true;
        }

        private void Update()
        {
            FireBullet();
        }

        private void LateUpdate()
        {
            RotateHead();
        }

        private void FireBullet()
        {
            if(m_controlScheme.Shoot.Value() > triggerSensivility) 
            {
                if(m_readyToShootTime <= Time.time && IsReadyToShoot())
                {
                    ShootByAnim();
                    m_readyToShootTime = Time.time + reloadTimeS;

                    ExecuteEvents.Execute<IUIEvents>(m_ui, null, (x, y) => x.OnActionUsed(ActionType.SHOOT, reloadTimeS));
                }
            }
        }

        private bool IsReadyToShoot()
        {
            return AnimatorQuery.IsInStateCompletely(m_animator, BossAnimation.StateNames.Move);
        }

        public abstract void ShootByAnim();

        public virtual void RotateHead()
        {
            head.LookAt(m_crossHairControl.Position);
            head.Rotate(0, -90, m_lookAtOffset);
            head.rotation *= m_initialHeadRotation;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(head.position, m_crossHairControl.Position);
        }
    }
}
