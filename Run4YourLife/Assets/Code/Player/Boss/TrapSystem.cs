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
        private GameObject skillA;
        
        [SerializeField]
        private GameObject skillX;
        
        [SerializeField]
        private GameObject skillY;
        
        [SerializeField]
        private GameObject skillB;
        
        [SerializeField]
        private float m_normalizedTimeToSpawnTrap = 0.2f;

        #endregion

        #region Members
        private float m_xReadyTime;
        private float m_yReadyTime;
        private float m_bReadyTime;
        private float m_aReadyTime;

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
                if (m_controlScheme.Skill1.Started() && (m_aReadyTime <= Time.time))
                {
                    m_aReadyTime = Time.time + CheckToSetElement(skillA, ActionType.A);             
                }
                else if (m_controlScheme.Skill2.Started() && (m_xReadyTime <= Time.time))
                {
                    m_xReadyTime = Time.time + CheckToSetElement(skillX, ActionType.X);
                }
                else if (m_controlScheme.Skill3.Started() && (m_yReadyTime <= Time.time))
                {
                    m_yReadyTime = Time.time + CheckToSetElement(skillY, ActionType.Y);
                }
                else if (m_controlScheme.Skill4.Started() && (m_bReadyTime <= Time.time))
                {
                    m_bReadyTime = Time.time + CheckToSetElement(skillB, ActionType.B);
                }
            }       
        }

        private bool IsReadyToUseSkill()
        {
            return AnimatorQuery.IsInStateCompletely(m_animator, BossAnimation.StateNames.Move);
        }

        private float CheckToSetElement(GameObject skill,ActionType type)
        {
            GameObject instance;

            if (!SkillCheckWorldAvailability(skill, out instance))
            {
                UnableToUseSkill();
                return 0.0f;
            }
            float skillReadyTime = PlaceSkillAtAnimation(instance);

            ExecuteEvents.Execute<IUIEvents>(m_ui, null, (x, y) => x.OnActionUsed(type, skillReadyTime));
            return skillReadyTime;
        }

        private bool SkillCheckWorldAvailability(GameObject skill,out GameObject instance)
        {
            instance = BossPoolManager.Instance.InstantiateBossElement(skill, m_crossHairControl.Position, false);
            return instance.GetComponent<SkillBase>().Check();
        }

        private float PlaceSkillAtAnimation(GameObject skill)
        {
            AudioManager.Instance.PlaySFX(m_castClip);
            m_animator.SetTrigger(BossAnimation.Triggers.Cast);
            
            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, BossAnimation.StateNames.Cast, m_normalizedTimeToSpawnTrap, () => PlaceSkillAtAnimationCallback(skill)));

            return skill.GetComponent<SkillBase>().Cooldown;
        }

        private void PlaceSkillAtAnimationCallback(GameObject skillInstance)
        {
            skillInstance.SetActive(true);
            skillInstance.GetComponent<SkillBase>().StartSkill();
        }


        private void UnableToUseSkill()
        {
            //Do something
        }
    }
}
