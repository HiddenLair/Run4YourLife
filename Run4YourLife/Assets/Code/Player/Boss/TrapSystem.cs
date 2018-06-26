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
                SkillBase.SkillSpawnData skillSpawnData = new SkillBase.SkillSpawnData() { position = m_crossHairControl.Position };
                if (m_controlScheme.Lightning.Started() && (m_lightningReadyTime <= Time.time) && m_lightningSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_lightningReadyTime = Time.time + m_lightningSkill.Cooldown;
                    ExecuteSkill(m_lightningSkill, ActionType.A, skillSpawnData);             
                }
                else if (m_controlScheme.EarthSpike.Started() && (m_earthSpikeReadyTime <= Time.time) && m_earthSpikeSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_earthSpikeReadyTime = Time.time + m_earthSpikeSkill.Cooldown;
                    ExecuteSkill(m_earthSpikeSkill, ActionType.X, skillSpawnData);
                }
                else if (m_controlScheme.Wind.Started() && (m_windReadyTime <= Time.time) && m_windSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_windReadyTime = Time.time + m_windSkill.Cooldown;
                    ExecuteSkill(m_windSkill, ActionType.Y, skillSpawnData);
                }
                else if (m_controlScheme.Bomb.Started() && (m_bombReadyTime <= Time.time) && m_bombSkill.CanBePlacedAt(ref skillSpawnData))
                {
                    m_bombReadyTime = Time.time + m_bombSkill.Cooldown;
                    ExecuteSkill(m_bombSkill, ActionType.B, skillSpawnData);
                } 
            }       
        }

        private bool IsReadyToUseSkill()
        {
            return AnimatorQuery.IsInStateCompletely(m_animator, BossAnimation.StateNames.Move);
        }

        private void ExecuteSkill(SkillBase skill, ActionType type, SkillBase.SkillSpawnData skillSpawnData)
        {
            GameObject instance = BossPoolManager.Instance.InstantiateBossElement(skill.gameObject, skillSpawnData.position, false);
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
    }
}
