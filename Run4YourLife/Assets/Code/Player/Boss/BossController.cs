﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


using Run4YourLife.Utils;
using Run4YourLife.InputManagement;
using Run4YourLife.UI;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.GameManagement;
using Run4YourLife.Player.Boss.Skills;

namespace Run4YourLife.Player
{

    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CrossHairControl))]
    public abstract class BossController : MonoBehaviour
    {

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
        private FXReceiver m_lightningCastReceiver;

        [SerializeField]
        private SkillBase m_earthSpikeSkill;

        [SerializeField]
        private FXReceiver m_earthSpikeCastReceiver;

        [SerializeField]
        private SkillBase m_windSkill;

        [SerializeField]
        private FXReceiver m_windCastReceiver;

        [SerializeField]
        private SkillBase m_bombSkill;

        [SerializeField]
        private FXReceiver m_bombCastReceiver;

        [SerializeField]
        [Tooltip("Offset in euler angles from wich the head will look, used to shoot with the mouth instead of the beak")]
        private Vector3 m_headLookAtOffset;

        [SerializeField]
        private float m_headLerpFactor;

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

        #endregion

        private float m_earthSpikeReadyTime;
        private float m_windReadyTime;
        private float m_bombReadyTime;
        private float m_lightningReadyTime;
        private float m_shootReadyTime;
        private float m_meleeReadyTime;
        private bool m_isHeadLocked;
        private bool m_isHeadLookAtAttachedToCrosshair = true;
        private Vector3 m_lookAtPosition;
        private PulsatingGlowDefaultShader glowController;
        private ParticlesColorChanger particlesController;
        protected Quaternion m_initialHeadRotation;

        protected BossControlScheme m_controlScheme;
        protected Animator m_animator;
        protected GameObject m_ui;
        protected CrossHairControl m_crossHairControl;

        protected bool IsHeadLocked
        {
            get { return m_isHeadLocked; }
            set
            {
                if (!value)
                {
                    m_isHeadLookAtAttachedToCrosshair = false;
                }
                m_isHeadLocked = value;
            }
        }

        private void Awake()
        {
            m_controlScheme = GetComponent<BossControlScheme>();
            m_animator = GetComponent<Animator>();
            m_crossHairControl = GetComponent<CrossHairControl>();
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            glowController = GetComponentInChildren<PulsatingGlowDefaultShader>();
            particlesController = GetComponentInChildren<ParticlesColorChanger>();

            Debug.Assert(m_ui != null);

            m_initialHeadRotation = m_headBone.rotation; // We have to store the starting position to in order to rotate it properly
        }

        private void OnEnable()
        {
            m_controlScheme.Active = true;
        }

        void Update()
        {
            if (IsReadyToAttack())
            {
                SkillBase.SkillSpawnData skillSpawnData = new SkillBase.SkillSpawnData() { position = m_crossHairControl.Position };
                if (m_controlScheme.Lightning.Started() && (m_lightningReadyTime <= Time.time) && m_lightningSkill.CheckAndRepositionSkillSpawn(ref skillSpawnData))
                {
                    m_lightningReadyTime = Time.time + m_lightningSkill.Cooldown;
                    ExecuteSkill(m_lightningSkill, ActionType.Y, skillSpawnData);
                    m_lightningCastReceiver.PlayFx(true);
                    ChangeGlowColor(Color.yellow, m_animator.GetCurrentAnimatorClipInfo(0).Length);
                }
                else if (m_controlScheme.EarthSpike.Started() && (m_earthSpikeReadyTime <= Time.time) && m_earthSpikeSkill.CheckAndRepositionSkillSpawn(ref skillSpawnData))
                {
                    m_earthSpikeReadyTime = Time.time + m_earthSpikeSkill.Cooldown;
                    ExecuteSkill(m_earthSpikeSkill, ActionType.A, skillSpawnData);
                    m_earthSpikeCastReceiver.PlayFx(true);
                    ChangeGlowColor(Color.green, m_animator.GetCurrentAnimatorClipInfo(0).Length);
                }
                else if (m_controlScheme.Wind.Started() && (m_windReadyTime <= Time.time) && m_windSkill.CheckAndRepositionSkillSpawn(ref skillSpawnData))
                {
                    m_windReadyTime = Time.time + m_windSkill.Cooldown;
                    ExecuteSkill(m_windSkill, ActionType.X, skillSpawnData);
                    m_windCastReceiver.PlayFx(true);
                    ChangeGlowColor(Color.cyan, m_animator.GetCurrentAnimatorClipInfo(0).Length);
                }
                else if (m_controlScheme.Bomb.Started() && (m_bombReadyTime <= Time.time) && m_bombSkill.CheckAndRepositionSkillSpawn(ref skillSpawnData))
                {
                    m_bombReadyTime = Time.time + m_bombSkill.Cooldown;
                    ExecuteSkill(m_bombSkill, ActionType.B, skillSpawnData);
                    m_bombCastReceiver.PlayFx(true);
                    ChangeGlowColor(Color.red, m_animator.GetCurrentAnimatorClipInfo(0).Length);
                }
                else if (m_controlScheme.Shoot.Started() && m_shootReadyTime <= Time.time)
                {
                    m_shootReadyTime = Time.time + m_shootCooldown;
                    ExecuteShoot();
                }
                else if (m_controlScheme.Melee.Started() && m_meleeReadyTime <= Time.time)
                {
                    m_meleeReadyTime = Time.time + m_meleeCooldown;
                    ExecuteMelee();
                    StartCoroutine(YieldHelper.WaitForSeconds(() => OnShootReady(), m_meleeCooldown));
                }
            }
        }

        private void ChangeGlowColor(Color color, float time)
        {
            if (glowController != null)
            {
                glowController.ChangeGlowColor(color, time);
            }
            if (particlesController != null)
            {
                particlesController.ChangeColor(color, time);
            }
        }

        protected virtual void OnShootReady() { }

        protected virtual void ExecuteMelee()
        {
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (x, y) => x.OnActionUsed(ActionType.MELEE, m_meleeCooldown));
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
            m_animator.SetTrigger(BossAnimation.Triggers.Cast);
            StartCoroutine(AnimationCallbacks.AfterStateAtNormalizedTime(m_animator, BossAnimation.StateNames.Move, m_normalizedTimeToSpawnTrap, () => PlaceSkillAtAnimationCallback(skill.gameObject, skillSpawnData)));
            AudioManager.Instance.PlaySFX(m_castClip);
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (x, y) => x.OnActionUsed(type, skill.Cooldown));
        }

        private void PlaceSkillAtAnimationCallback(GameObject prefab, SkillBase.SkillSpawnData skillSpawnData)
        {
            GameObject instance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(prefab, skillSpawnData.position, Quaternion.identity);

            SimulateChildOf simulateChildOf = instance.GetComponent<SimulateChildOf>();
            if(skillSpawnData.parent != null)
            {
                simulateChildOf.Parent = skillSpawnData.parent;
            }

            instance.SetActive(true);
            instance.GetComponent<SkillBase>().StartSkill();
        }

        protected virtual void ExecuteShoot()
        {
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (x, y) => x.OnActionUsed(ActionType.SHOOT, m_shootCooldown));
        }

        private void RotateHead()
        {
            if (!m_isHeadLocked)
            {
                if (m_isHeadLookAtAttachedToCrosshair)
                {
                    m_lookAtPosition = m_crossHairControl.Position;
                }
                else if (Vector3.SqrMagnitude(m_lookAtPosition - m_crossHairControl.Position) < 0.07f)
                {
                    m_isHeadLookAtAttachedToCrosshair = true;
                    m_lookAtPosition = m_crossHairControl.Position;
                }
                else
                {
                    m_lookAtPosition = Vector3.Lerp(m_lookAtPosition, m_crossHairControl.Position, m_headLerpFactor * Time.deltaTime);
                }
            }

            m_headBone.LookAt(m_lookAtPosition);
            m_headBone.rotation *= Quaternion.Euler(m_headLookAtOffset.x, m_headLookAtOffset.y, m_headLookAtOffset.z) * m_initialHeadRotation;
        }
    }
}
