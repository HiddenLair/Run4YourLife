﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Run4YourLife.UI;
using Run4YourLife.Input;

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
        [Range(0,1)]
        private float crossHairSensivility = 0.2f;

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

        [SerializeField]
        private float maxTrapWidth;

        [SerializeField]
        private float maxTrapHeight;

        #endregion

        #region Variables

        Type currentType = Type.TRAP;
        Ready ready;
        BossControlScheme bossControlScheme;
        private Animator anim;
        private GameObject uiManager;
        private bool active = true;
        private bool triggering = false;
        private Color enabledColor;
        private Material crossHairMat;

        #endregion

        #region Timers

        private float xButtonCooldown = 0.0f;
        private float yButtonCooldown = 0.0f;
        private float bButtonCooldown = 0.0f;
        private float aButtonCooldown = 0.0f;

        #endregion

        private void Awake()
        {
            crossHairMat = crossHair.GetComponent<Renderer>().material;
            enabledColor = crossHairMat.color;
            currentType = (Type)((int)phase % 2);
            ready = GetComponent<Ready>();
            anim = GetComponent<Animator>();
            bossControlScheme = GetComponent<BossControlScheme>();
            uiManager = GameObject.FindGameObjectWithTag("UI");
        }

        private void OnDisable()
        {
            crossHairMat.color = enabledColor;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            CheckForScreenLimits();
            if (!triggering)
            {
                CheckIfSpaceAvailable();
            }

            if (ready.Get() && active) {
                CheckToSetElement();
            }

            if (phase == Phase.Phase3)
            {
                if (bossControlScheme.nextSet.Started())
                {
                    currentType = Type.TRAP;
                }

                if (bossControlScheme.previousSet.Started())
                {
                    currentType = Type.SKILL;
                }
            }
        }

        void Move()
        {
            float xInput = bossControlScheme.moveTrapIndicatorHorizontal.Value();
            if(Mathf.Abs(xInput) >= crossHairSensivility)
            {
                if(xInput > 0)
                {
                    Vector3 temPos = crossHair.transform.position;
                    temPos.x += crossHairSpeed * Time.deltaTime;
                    crossHair.transform.position = temPos;
                }
                else
                {
                    Vector3 temPos = crossHair.transform.position;
                    temPos.x -= crossHairSpeed * Time.deltaTime;
                    crossHair.transform.position = temPos;
                }
            }

            float yInput = bossControlScheme.moveTrapIndicatorVertical.Value();
            if(Mathf.Abs(yInput) >= crossHairSensivility)
            {
                if(yInput > 0)
                {
                    Vector3 temPos = crossHair.transform.position;
                    temPos.y += crossHairSpeed * Time.deltaTime;
                    crossHair.transform.position = temPos;
                }
                else
                {
                    Vector3 temPos = crossHair.transform.position;
                    temPos.y -= crossHairSpeed * Time.deltaTime;
                    crossHair.transform.position = temPos;
                }
            }
        }

        void CheckForScreenLimits()
        {

            Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Mathf.Abs(Camera.main.transform.position.z - crossHair.transform.position.z)));
            Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth * screenLeftLimitPercentaje, Camera.main.pixelHeight * screenBottomLimitPercentaje, Mathf.Abs(Camera.main.transform.position.z - crossHair.transform.position.z)));

            //Horizontal
            if (crossHair.transform.position.x > screenTopRight.x)
            {
                Vector3 tempPos = crossHair.transform.position;
                tempPos.x = screenTopRight.x;
                crossHair.transform.position = tempPos;
            }
            else if (crossHair.transform.position.x < screenBottomLeft.x)
            {
                Vector3 tempPos = crossHair.transform.position;
                tempPos.x = screenBottomLeft.x;
                crossHair.transform.position = tempPos;
            }

            //Vertical
            if(crossHair.transform.position.y > screenTopRight.y)
            {
                Vector3 tempPos = crossHair.transform.position;
                tempPos.y = screenTopRight.y;
                crossHair.transform.position = tempPos;
            }else if (crossHair.transform.position.y < screenBottomLeft.y)
            {
                Vector3 tempPos = crossHair.transform.position;
                tempPos.y = screenBottomLeft.y;
                crossHair.transform.position = tempPos;
            }
        }

        void CheckIfSpaceAvailable()
        {
            //Check if we have space to place the bigger trap
            RaycastHit info;
            if(Physics.Raycast(crossHair.transform.position, Vector3.up,out info, maxTrapHeight, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
            }
            else if (Physics.Raycast(crossHair.transform.position, Vector3.right, out info, maxTrapWidth/2, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
            }
            else if (Physics.Raycast(crossHair.transform.position, -Vector3.right, out info, maxTrapWidth / 2, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
            }
            else
            {
                Activate();
            }

        }

        void Deactivate()
        {
            //There is no space to place the trap
            crossHairMat.color = Color.gray;
            active = false;
        }

        void Activate()
        {
            crossHairMat.color = enabledColor;
            active = true;
        }

        void CheckToSetElement()
        {
            if (bossControlScheme.skill1.Started() && (aButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapA, skillA);
                aButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_A, aButtonCooldown));
            }
            else if (bossControlScheme.skill2.Started() && (xButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapX, skillX);
                xButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_X, buttonCooldown));
            }
            else if (bossControlScheme.skill3.Started() && (yButtonCooldown <= Time.time))
            {
                float buttonCooldown = SetElement(trapY, skillY);
                yButtonCooldown = Time.time + buttonCooldown;

                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.TRAP_Y, buttonCooldown));
            }
            else if (bossControlScheme.skill4.Started() && (bButtonCooldown <= Time.time))
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
                cooldown = skill.GetComponent<CooldownIndicator>().cooldown;
                Vector3 temp = crossHair.transform.position;
                var g = Instantiate(skill, temp, skill.GetComponent<Transform>().rotation);
            }
            else
            {
                cooldown = trap.GetComponent<CooldownIndicator>().cooldown;
                Vector3 temp = crossHair.transform.position;
                GameObject g = Instantiate(trap, temp, trap.GetComponent<Transform>().rotation);
                g.GetComponentInChildren<Collider>().enabled = false;
                Color actualC = g.GetComponentInChildren<Renderer>().material.color;
                actualC.a = 0;
                g.GetComponentInChildren<Renderer>().material.color = actualC;
                StartCoroutine(SpawnTrapDelayed(g, trapDelaySpawn));
            }
            return cooldown;
        }

        IEnumerator SpawnTrapDelayed(GameObject g, float delay)
        {
            float fps = 1 / Time.deltaTime;
            float alphaPerFrame = 1 / (delay * fps);
            Color temp = g.GetComponentInChildren<Renderer>().material.color;
            while (temp.a < 1)
            {
                temp.a += alphaPerFrame;
                g.GetComponentInChildren<Renderer>().material.color = temp;
                yield return 0;//Wait 1 frame
            }
            g.GetComponentInChildren<Collider>().enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Deactivate();
            triggering = true;
        }

        private void OnTriggerExit(Collider other)
        {
            Activate();
            triggering = false;
        }
    }
}