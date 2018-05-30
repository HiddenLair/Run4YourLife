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
    [RequireComponent(typeof(Ready))]
    [RequireComponent(typeof(BossControlScheme))]
    [RequireComponent(typeof(CrossHairControl))]
    public class TrapSystem : MonoBehaviour
    {
        #region Enums

        public enum Phase { Phase1, Phase2, Phase3 };
        private enum Type { TRAP, SKILL };

        #endregion

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
        private Phase phase;

        [SerializeField]
        private GameObject skillA;
        [SerializeField]
        private GameObject trapA;
        [SerializeField]
        private GameObject skillX;
        [SerializeField]
        private GameObject trapX;
        [SerializeField]
        private GameObject skillY;
        [SerializeField]
        private GameObject trapY;
        [SerializeField]
        private GameObject skillB;
        [SerializeField]
        private GameObject trapB;

        #endregion

        #region Variables

        Type currentType = Type.TRAP;
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
            currentType = (Type)((int)phase % 2);
            ready = GetComponent<Ready>();
            anim = GetComponent<Animator>();
            bossControlScheme = GetComponent<BossControlScheme>();
            crossHairControl = GetComponent<CrossHairControl>();
            uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
        }

        void Update()
        {
            Move();

            if (ready.Get() && crossHairControl.IsOperative && !trapCooldownBool)
            {
                trapCooldownBool = true;
                CheckToSetElement();
            }

            if (phase == Phase.Phase3)
            {
                if (bossControlScheme.NextSet.Started())
                {
                    currentType = Type.TRAP;
                }

                if (bossControlScheme.PreviousSet.Started())
                {
                    currentType = Type.SKILL;
                }
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

        void CheckToSetElement()
        {
            if (bossControlScheme.Skill1.Started() && (aButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapA, skillA);
                aButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_A, buttonCooldown));
            }
            else if (bossControlScheme.Skill2.Started() && (xButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapX, skillX);
                xButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_X, buttonCooldown));
            }
            else if (bossControlScheme.Skill3.Started() && (yButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapY, skillY);
                yButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_Y, buttonCooldown));
            }
            else if (bossControlScheme.Skill4.Started() && (bButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapB, skillB);
                bButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_B, buttonCooldown));
            }
            else
            {
                trapCooldownBool = false;
            }
        }

        float SetElement(GameObject trap, GameObject skill)
        {
            AudioManager.Instance.PlaySFX(m_castClip);
            anim.SetTrigger("Casting");
            trapCooldownBool = false;

            float cooldown = 0.0f ;
            if (currentType == Type.SKILL)
            {
                cooldown = skill.GetComponent<SkillBase>().Cooldown;
                StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(anim, "Cast", timeToSpawnTrapsFromAnim, () => SetElementCallback(skill)));

            }
            else
            {
                cooldown = trap.GetComponent<TrapBase>().Cooldown;
                StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(anim, "Cast", timeToSpawnTrapsFromAnim,()=> SetElementCallback(trap)));
            }
            return cooldown;
        }

        void SetElementCallback(GameObject gameObject)
        {
            BossPoolManager.Instance.InstantiateBossElement(gameObject, crossHairControl.Position);
        }
    }
}
