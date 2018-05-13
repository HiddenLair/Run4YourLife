using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.UI;
using Run4YourLife.InputManagement;
using Run4YourLife.Interactables;
using Run4YourLife.GameManagement;

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

        // Update is called once per frame
        void Update()
        {
            Move();

            if (ready.Get() && crossHairControl.IsCrossHairActive()) {
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
            Vector2 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, Mathf.Abs(mainCamera.transform.position.z - crossHairControl.GetPosition().z)));//crossHair.transform.position.z)));
            Vector2 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth * screenLeftLimitPercentaje, mainCamera.pixelHeight * screenBottomLimitPercentaje, Mathf.Abs(mainCamera.transform.position.z - crossHairControl.GetPosition().z))); //crossHair.transform.position.z)));

            Vector3 clampedPosition = crossHairControl.GetPosition();
            clampedPosition.x = Mathf.Clamp(crossHairControl.GetPosition().x, screenBottomLeft.x, screenTopRight.x);
            clampedPosition.y = Mathf.Clamp(crossHairControl.GetPosition().y, screenBottomLeft.y, screenTopRight.y);
            //crossHairControl.ChangePosition(clampedPosition);
        }

        void CheckToSetElement()
        {
            if (bossControlScheme.Skill1.Started() && (aButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapA, skillA);
                aButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_A, aButtonCooldown));
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
        }

        float SetElement(GameObject trap, GameObject skill)
        {
            anim.SetTrigger("Casting");
            float cooldown = 0.0f ;
            if (currentType == Type.SKILL)
            {
                cooldown = skill.GetComponent<SkillBase>().Cooldown;
                AnimationPlayOnTimeManager.Instance.PlayOnAnimation(anim, "Cast", timeToSpawnTrapsFromAnim, () => SetElementCallback(skill));
            }
            else
            {
                cooldown = trap.GetComponent<TrapBase>().Cooldown;
                AnimationPlayOnTimeManager.Instance.PlayOnAnimation(anim,"Cast",timeToSpawnTrapsFromAnim,()=> SetElementCallback(trap));
            }
            return cooldown;
        }

        void SetElementCallback(GameObject g)
        {
            Vector3 temp = crossHairControl.GetPosition();
            Instantiate(g, temp, g.GetComponent<Transform>().rotation);
        }
    }
}
