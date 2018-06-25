using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.UI;
using Run4YourLife.InputManagement;
using Run4YourLife.Interactables;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(CrossHairControl))]
    public class TrapSystem : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private AudioClip m_castClip;

        [SerializeField]
        private SkillBase m_lightningSkill;
        
        [SerializeField]
        private SkillBase m_earthSpikeSkill;
        
        [SerializeField]
        private SkillBase m_windSkill;
        
        [SerializeField]
        private SkillBase m_bombSkill;
        
        [SerializeField]
        private float m_normalizedTimeToSpawnTrap = 0.2f;

        #endregion

        #region Members
        private float m_earthSpikeReadyTime;
        private float m_windReadyTime;
        private float m_bombReadyTime;
        private float m_lightningReadyTime;

        private BossControlScheme m_controlScheme;
        private Animator m_animator;
        private GameObject m_ui;
        private CrossHairControl m_crossHairControl;

        #endregion

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_controlScheme = GetComponent<BossControlScheme>();
            m_crossHairControl = GetComponent<CrossHairControl>();
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
        }

        private void Update()
        {
            if (IsReadyToUseSkill())
            {
                if (m_controlScheme.Skill1.Started() && (m_lightningReadyTime <= Time.time) && m_lightningSkill.CanBePlacedAtPosition(m_crossHairControl.Position))
                {
                    m_lightningReadyTime = Time.time + m_lightningSkill.Cooldown;
                    ExecuteSkill(m_lightningSkill, ActionType.A);             
                }
                else if (m_controlScheme.Skill2.Started() && (m_earthSpikeReadyTime <= Time.time) && m_earthSpikeSkill.CanBePlacedAtPosition(m_crossHairControl.Position))
                {
                    m_earthSpikeReadyTime = Time.time + m_earthSpikeSkill.Cooldown;
                    ExecuteSkill(m_earthSpikeSkill, ActionType.X);
                }
                else if (m_controlScheme.Skill3.Started() && (m_windReadyTime <= Time.time) && m_windSkill.CanBePlacedAtPosition(m_crossHairControl.Position))
                {
                    m_windReadyTime = Time.time + m_windSkill.Cooldown;
                    ExecuteSkill(m_windSkill, ActionType.Y);
                }
                else if (m_controlScheme.Skill4.Started() && (m_bombReadyTime <= Time.time) && m_bombSkill.CanBePlacedAtPosition(m_crossHairControl.Position))
                {
                    m_bombReadyTime = Time.time + m_bombSkill.Cooldown;
                    ExecuteSkill(m_bombSkill, ActionType.B);
                }
            }       
        }

        private bool IsReadyToUseSkill()
        {
            return AnimatorQuery.IsInStateCompletely(m_animator, BossAnimation.StateNames.Move);
        }

        private void ExecuteSkill(SkillBase skill, ActionType type)
        {
            GameObject instance = BossPoolManager.Instance.InstantiateBossElement(skill.gameObject, m_crossHairControl.Position, false);
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
    }
}
