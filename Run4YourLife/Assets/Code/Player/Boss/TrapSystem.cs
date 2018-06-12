﻿using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.UI;
using Run4YourLife.InputManagement;
using Run4YourLife.Interactables;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Ready))]
    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(CrossHairControl))]
    public class TrapSystem : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private AudioClip m_castClip;

        [SerializeField]
        [Range(0, 1)]
        private float screenLeftLimitPercentaje = 0.2f;

        [SerializeField]
        [Range(0, 1)]
        private float screenBottomLimitPercentaje = 0.2f;

        [SerializeField]
        private GameObject skillA;
        [SerializeField]
        private GameObject skillX;
        [SerializeField]
        private GameObject skillY;
        [SerializeField]
        private GameObject skillB;

        #endregion

        #region Variables

        Ready ready;
        BossControlScheme bossControlScheme;
        private Animator anim;
        private GameObject uiManager;
        private CrossHairControl crossHairControl;

        private float timeToSpawnTrapsFromAnim = 0.2f;
        private bool trapCooldownBool = false;
        #endregion

        #region Timers

        private float xButtonCooldown = 0.0f;
        private float yButtonCooldown = 0.0f;
        private float bButtonCooldown = 0.0f;
        private float aButtonCooldown = 0.0f;

        #endregion

        private void Awake()
        {
            ready = GetComponent<Ready>();
            anim = GetComponent<Animator>();
            bossControlScheme = GetComponent<BossControlScheme>();
            crossHairControl = GetComponent<CrossHairControl>();
            uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
        }

        void Update()
        {
            Move();

            if (ready.Get() && !trapCooldownBool)
            {
                trapCooldownBool = true;
                SelectElementToSet();
                trapCooldownBool = false;
            }       
        }

        void Move()
        {
            float xInput = bossControlScheme.MoveTrapIndicatorHorizontal.Value();
            float yInput = bossControlScheme.MoveTrapIndicatorVertical.Value();
            Vector3 input = new Vector3(xInput, yInput);

            crossHairControl.Translate(input);
            ClampPositionInsideScreen();
        }

        void ClampPositionInsideScreen()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;

            Vector3 screenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, Mathf.Abs(mainCamera.transform.position.z - crossHairControl.Position.z)));
            Vector3 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(screenLeftLimitPercentaje, screenBottomLimitPercentaje, Mathf.Abs(mainCamera.transform.position.z - crossHairControl.Position.z)));

            Vector3 clampedPosition = crossHairControl.Position;
            clampedPosition.x = Mathf.Clamp(crossHairControl.Position.x, screenBottomLeft.x, screenTopRight.x);
            clampedPosition.y = Mathf.Clamp(crossHairControl.Position.y, screenBottomLeft.y, screenTopRight.y);

            crossHairControl.ChangePosition(clampedPosition);
        }

        void SelectElementToSet()
        {
            if (bossControlScheme.Skill1.Started() && (aButtonCooldown <= Time.time))
            {
                aButtonCooldown = Time.time + CheckToSetElement(skillA, ActionType.TRAP_A);             
            }
            else if (bossControlScheme.Skill2.Started() && (xButtonCooldown <= Time.time))
            {
                xButtonCooldown = Time.time + CheckToSetElement(skillX, ActionType.TRAP_X);
            }
            else if (bossControlScheme.Skill3.Started() && (yButtonCooldown <= Time.time))
            {
                yButtonCooldown = Time.time + CheckToSetElement(skillY, ActionType.TRAP_Y);
            }
            else if (bossControlScheme.Skill4.Started() && (bButtonCooldown <= Time.time))
            {
                bButtonCooldown = Time.time + CheckToSetElement(skillB, ActionType.TRAP_B);
            }
        }

        float CheckToSetElement(GameObject skill,ActionType type)
        {
            GameObject instance;

            if (!SkillCheckWorldAvailability(skill, out instance))
            {
                CanNotPlace();
                return 0.0f;
            }
            float buttonCooldown = SetElement(instance);

            ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(type, buttonCooldown));
            return buttonCooldown;
        }

        float SetElement(GameObject skill)
        {
            AudioManager.Instance.PlaySFX(m_castClip);
            anim.SetTrigger("Casting");
            
            float cooldown = 0.0f ;

            cooldown = skill.GetComponent<SkillBase>().Cooldown;
            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(anim, "Cast", timeToSpawnTrapsFromAnim, () => SetElementCallback(skill)));

            return cooldown;
        }

        void SetElementCallback(GameObject gameObject)
        {
            gameObject.SetActive(true);
            crossHairControl.TotalUnlock();
        }

        bool SkillCheckWorldAvailability(GameObject skill,out GameObject instance)
        {
           
            instance = BossPoolManager.Instance.InstantiateBossElement(skill, crossHairControl.Position,false);
            bool ret = instance.GetComponent<SkillBase>().Check();
            if (ret)
            {
                crossHairControl.TotalLock();
            }
            return ret;
        }

        void CanNotPlace()
        {
            //Do something
        }
    }
}
