using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Input;
using UnityEngine.EventSystems;
using Run4YourLife.UI;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Ready))]
    [RequireComponent(typeof(BossControlScheme))]
    public abstract class Shoot : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private Transform head;

        [SerializeField]
        protected Transform shootInitZone;

        [SerializeField]
        protected Transform crossHair;

        [SerializeField]
        [Range(0, 1)]
        private float triggerSensivility = 0.2f;

        [SerializeField]
        private float reloadTimeS;

        [SerializeField]
        protected AudioClip sfx;

        [SerializeField]
        private AnimationClip animShoot;

        [SerializeField]
        private float shootAnimTimeVariation;

        #endregion

        private float currentTimeS = 0;
        private bool alreadyPressed = false;
        private Quaternion initialRotation;

        private Ready ready;
        private BossControlScheme controlScheme;

        protected Animator animator;
        protected AudioSource audioSource;

        private GameObject uiManager;

        private void Awake()
        {
            currentTimeS = Time.time + reloadTimeS;

            GetComponents();

            uiManager = GameObject.FindGameObjectWithTag("UI");

            head.LookAt(crossHair);
        }

        private void Start()
        {
            controlScheme.Active = true;
            initialRotation = head.rotation;
        }

        void Update()
        {
            //head.LookAt(crossHair);

            Verify();
        }

        private void LateUpdate()
        {
            Quaternion lookRotation = Quaternion.LookRotation(crossHair.position - head.position);
            head.rotation = lookRotation * initialRotation;
        }

        private void Verify()
        {
            if (ready.Get())
            {
                if (controlScheme.shoot.Value() > triggerSensivility)
                {
                    if (currentTimeS <= Time.time && !alreadyPressed)
                    {
                        animator.SetTrigger("Shoot");
                        Invoke("OnSuccess", animShoot.length - shootAnimTimeVariation);

                        currentTimeS = Time.time + reloadTimeS;
                        alreadyPressed = true;

                        ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.SHOOT, reloadTimeS));
                    }
                }
                else
                {
                    alreadyPressed = false;
                }
            }
        }

        protected virtual void GetComponents()
        {
            ready = GetComponent<Ready>();
            controlScheme = GetComponent<BossControlScheme>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        protected abstract void OnSuccess();
    }
}
