using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.UI;
using Run4YourLife.InputManagement;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public abstract class Melee : MonoBehaviour
    {
        #region Editor variables

        [SerializeField]
        [Range(0, 1)]
        private float triggerSensivility = 0.2f;

        [SerializeField]
        private float reloadTimeS;

        [SerializeField]
        protected AudioClip m_meleeClip;

        #endregion

        private float m_readyToMeleeTime;
        private bool m_buttonWasReleased;

        private BossControlScheme m_controlScheme;
        private GameObject m_ui;
        protected Animator m_animator;

        protected virtual void Awake()
        {
            m_readyToMeleeTime = Time.time + reloadTimeS;
            m_controlScheme = GetComponent<BossControlScheme>();
            m_animator = GetComponent<Animator>();
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);
        }

        private void Start()
        {
            m_controlScheme.Active = true;
        }

        private void Update()
        {
            CheckExecuteMelee();
        }

        private void CheckExecuteMelee()
        {
            if(m_controlScheme.Melee.Value() > triggerSensivility)
            {
                if(m_buttonWasReleased && m_readyToMeleeTime <= Time.time && IsReadyToMelee())
                {
                    m_buttonWasReleased = false;
                    m_readyToMeleeTime = Time.time + reloadTimeS;

                    ExecuteMelee();
                    ExecuteEvents.Execute<IUIEvents>(m_ui, null, (x, y) => x.OnActionUsed(ActionType.MELE, reloadTimeS));
                }
            }
            else
            {
                m_buttonWasReleased = true;
            }
        }

        private bool IsReadyToMelee()
        {
            return AnimatorQuery.IsInStateCompletely(m_animator, BossAnimation.StateNames.Move);
        }

        protected abstract void ExecuteMelee();
    }
}