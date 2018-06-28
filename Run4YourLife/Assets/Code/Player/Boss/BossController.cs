using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


using Run4YourLife.Utils;
using Run4YourLife.InputManagement;
using Run4YourLife.UI;
using Run4YourLife.GameManagement.AudioManagement;
using System;

namespace Run4YourLife.Player
{

    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CrossHairControl))]
    public abstract class BossController : MonoBehaviour {

        #region Inspector
        [SerializeField]
        private float m_meleeCooldown;

        [SerializeField]
        private float m_shootCooldown;

        [SerializeField]
        private float m_normalizedTimeToSpawnTrap;

        [SerializeField]
        private SkillBase m_lightningSkill;
        
        [SerializeField]
        private SkillBase m_earthSpikeSkill;
        
        [SerializeField]
        private SkillBase m_windSkill;
        
        [SerializeField]
        private SkillBase m_bombSkill;

        [SerializeField]
        [Tooltip("Offset in euler angles from wich the head will look, used to shoot with the mouth instead of the beak")]
        private Vector3 m_headLookAtOffset;

        [SerializeField]
        private Transform m_headBone;

        [SerializeField]
        protected Transform m_shotSpawn;

        [SerializeField]
        [Tooltip("Audio clip that plays when the boss uses a skill")]
        private AudioClip m_castClip;

        [SerializeField]
        [Tooltip("Audio clip that plays when the boss shoots a bullet")]
        protected AudioClip m_shotClip;

        [SerializeField]
        protected AudioClip m_meleeClip;

        [SerializeField]
        protected GameObjectPool m_gameObjectPool;

        #endregion

        private float m_earthSpikeReadyTime;
        private float m_windReadyTime;
        private float m_bombReadyTime;
        private float m_lightningReadyTime;
        private float m_shootReadyTime;
        private float m_meleeReadyTime;
        protected Quaternion m_initialHeadRotation;

        protected BossControlScheme m_controlScheme;
        protected Animator m_animator;
        protected GameObject m_ui;
        protected CrossHairControl m_crossHairControl;

        private void Awake()
        {
            m_controlScheme = GetComponent<BossControlScheme>();
            m_animator = GetComponent<Animator>();
            m_crossHairControl = GetComponent<CrossHairControl>();
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);

            m_initialHeadRotation = m_headBone.rotation; // We have to store the starting position to in order to rotate it properly
        }

        private void Start()
        {
            m_controlScheme.Active = true;
        }

        void Update() {
            if(IsReadyToAttack())
            {
                SkillBase.SkillSpawnData skillSpawnData = new SkillBase.SkillSpawnData() { position = m_crossHairControl.Position };
                if (m_controlScheme.Lightning.Started() && (m_lightningReadyTime <= Time.time) && m_lightningSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_lightningReadyTime = Time.time + m_lightningSkill.Cooldown;
                    ExecuteSkill(m_lightningSkill, ActionType.Y, skillSpawnData);             
                }
                else if (m_controlScheme.EarthSpike.Started() && (m_earthSpikeReadyTime <= Time.time) && m_earthSpikeSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_earthSpikeReadyTime = Time.time + m_earthSpikeSkill.Cooldown;
                    ExecuteSkill(m_earthSpikeSkill, ActionType.A, skillSpawnData);
                }
                else if (m_controlScheme.Wind.Started() && (m_windReadyTime <= Time.time) && m_windSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_windReadyTime = Time.time + m_windSkill.Cooldown;
                    ExecuteSkill(m_windSkill, ActionType.X, skillSpawnData);
                }
                else if (m_controlScheme.Bomb.Started() && (m_bombReadyTime <= Time.time) && m_bombSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_bombReadyTime = Time.time + m_bombSkill.Cooldown;
                    ExecuteSkill(m_bombSkill, ActionType.B, skillSpawnData);
                } 
                else if(m_controlScheme.Shoot.BoolValue() && m_shootReadyTime <= Time.time)
                {
                    m_shootReadyTime = Time.time + m_shootCooldown;
                    ExecuteShoot();
                }
                else if(m_controlScheme.Melee.BoolValue() && m_meleeReadyTime <= Time.time)
                {
                    m_meleeReadyTime = Time.time + m_meleeCooldown;
                    ExecuteMelee();
                }
            }
        }

        protected virtual void ExecuteMelee()
        {
            ExecuteEvents.Execute<IUIEvents>(m_ui, null, (x, y) => x.OnActionUsed(ActionType.MELE, m_meleeCooldown));
        }

        private void LateUpdate()
        {
            RotateHead();
        }
        
        private bool IsReadyToAttack()
        {
            return AnimatorQuery.IsInStateCompletely(m_animator, BossAnimation.StateNames.Move);
        }

        private void ExecuteSkill(SkillBase skill, ActionType type, SkillBase.SkillSpawnData skillSpawnData)
        {
            GameObject instance = m_gameObjectPool.GetAndPosition(skill.gameObject, skillSpawnData.position, Quaternion.identity);
            if(skillSpawnData.parent != null)
            {
                instance.transform.SetParent(skillSpawnData.parent);
            }

            m_animator.SetTrigger(BossAnimation.Triggers.Cast);
            StartCoroutine(AnimationCallbacks.AfterStateAtNormalizedTime(m_animator, BossAnimation.StateNames.Move, m_normalizedTimeToSpawnTrap, () => PlaceSkillAtAnimationCallback(instance)));
            AudioManager.Instance.PlaySFX(m_castClip);
            ExecuteEvents.Execute<IUIEvents>(m_ui, null, (x, y) => x.OnActionUsed(type, skill.Cooldown));
        }

        private void PlaceSkillAtAnimationCallback(GameObject instance)
        {
            instance.SetActive(true);
            instance.GetComponent<SkillBase>().StartSkill();
        }


        protected virtual void ExecuteShoot()
        {
            ExecuteEvents.Execute<IUIEvents>(m_ui, null, (x, y) => x.OnActionUsed(ActionType.SHOOT, m_shootCooldown));
        }

        private void RotateHead()
        {
            m_headBone.LookAt(m_crossHairControl.Position);
            //m_headBone.Rotate(0, -90, m_headLookAtOffset);
            m_headBone.rotation *= Quaternion.Euler(m_headLookAtOffset.x, m_headLookAtOffset.y, m_headLookAtOffset.z) * m_initialHeadRotation;
        }
    }
}
