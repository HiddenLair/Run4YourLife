using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

using Run4YourLife.UI;
using Run4YourLife.Input;
using Run4YourLife.Interactables;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Ready))]
    [RequireComponent(typeof(BossControlScheme))]
    public class TrapSystem : MonoBehaviour
    {

        #region Enums

        public enum Phase { Phase1, Phase2, Phase3 };
        private enum Type { TRAP, SKILL };

        #endregion

        #region Inspector

        [SerializeField]
        private GameObject crossHair;

        [SerializeField]
        private float crossHairSpeed;

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
        [SerializeField]
        private float trapDelaySpawn;

        #endregion

        #region Variables

        Type currentType = Type.TRAP;
        Ready ready;
        BossControlScheme bossControlScheme;
        private Animator anim;
        private GameObject uiManager;
        private bool animTriggerSetElement = false;


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
            uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
        }

        // Update is called once per frame
        void Update()
        {
            Move();

            if (ready.Get() && crossHair.GetComponent<CrossHair>().GetActive()) {
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

            crossHair.transform.Translate(input * crossHairSpeed * Time.deltaTime);
            ClampPositionInsideScreen();
        }

        void ClampPositionInsideScreen()
        {
            Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Mathf.Abs(Camera.main.transform.position.z - crossHair.transform.position.z)));
            Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth * screenLeftLimitPercentaje, Camera.main.pixelHeight * screenBottomLimitPercentaje, Mathf.Abs(Camera.main.transform.position.z - crossHair.transform.position.z)));

            Vector3 clampedPosition = crossHair.transform.position;
            clampedPosition.x = Mathf.Clamp(crossHair.transform.position.x, screenBottomLeft.x, screenTopRight.x);
            clampedPosition.y = Mathf.Clamp(crossHair.transform.position.y, screenBottomLeft.y, screenTopRight.y);
            crossHair.transform.position = clampedPosition;
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
                cooldown = skill.GetComponent<CooldownIndicator>().cooldown; // TODO: Refactor this
                StartCoroutine(WaitForAnim(skill));
            }
            else
            {
                cooldown = trap.GetComponent<TrapBase>().Cooldown;
                StartCoroutine(WaitForAnim(trap));
            }
            return cooldown;
        }

        IEnumerator WaitForAnim(GameObject g)
        {
            yield return new WaitUntil(() => animTriggerSetElement);
            Vector3 temp = crossHair.transform.position;
            Instantiate(g, temp, g.GetComponent<Transform>().rotation);
            animTriggerSetElement = false;
        }

        public void SetElementTriggered()
        {
            animTriggerSetElement = true;
        }
    }
}
