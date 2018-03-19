using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;
using UnityEngine.EventSystems;

using Run4YourLife.UI;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public class Boss3 : MonoBehaviour
    {
        //Shoot
        public GameObject laser;
        public float laserDuration;
        public float rotationSpeed;
        public AnimationClip animShoot;
        public Transform shootMarker;
        public Transform bulletStartingPoint;
        public float reload;

        //Mele
        public GameObject mele;
        public float meleSpeed;
        public Transform meleStartingPoint;
        public float meleReload;

        private float bulletTimer;
        private float meleTimer;
        private bool meleBeingPressed = false;
        private const float shootAnimTimeVariation = 0f;

        //Head rotation limits
        private const float topHeadRotation = 55;
        private const float bottomHeadRotation = -7;

        private BossControlScheme bossControlScheme;
        private Laser trapSetter;
        private Animator anim;

        private GameObject uiManager;

        private void Awake()
        {
            bossControlScheme = GetComponent<BossControlScheme>();
            bulletTimer = reload;
            meleTimer = meleReload;
            anim = GetComponent<Animator>();
            trapSetter = GetComponent<Laser>();

            uiManager = GameObject.FindGameObjectWithTag("UI");
        }

        private void Start()
        {
            bossControlScheme.Active = true;
        }

        void Update()
        {
            bulletTimer = Mathf.Min(bulletTimer + Time.deltaTime, reload);

            meleTimer = Mathf.Min(meleTimer + Time.deltaTime, meleReload);

            if (trapSetter.isReadyForAction)
            {
                ShootVerification();
            }

            if (trapSetter.isReadyForAction)
            {
                MeleVerification();
            }

        }

        void ShootVerification()
        {
            float yInput = bossControlScheme.moveLaserVertical.Value();
            if (Mathf.Abs(yInput) > 0.2)
            {
                if (topHeadRotation >= shootMarker.localEulerAngles.z || 360 + bottomHeadRotation <= shootMarker.localEulerAngles.z)
                {
                    if (yInput < 0)
                    {
                        Quaternion initRotation = shootMarker.rotation;
                        Quaternion temp = initRotation * Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
                        shootMarker.rotation = temp;
                        if (shootMarker.localEulerAngles.z > topHeadRotation && shootMarker.localEulerAngles.z < 360 + bottomHeadRotation)
                        {
                            shootMarker.rotation = initRotation;
                        }
                    }
                    else
                    {
                        Quaternion initRotation = shootMarker.rotation;
                        Quaternion temp = initRotation * Quaternion.Euler(0, 0, -rotationSpeed * Time.deltaTime);
                        shootMarker.rotation = temp;
                        if (shootMarker.localEulerAngles.z < 360 + bottomHeadRotation && shootMarker.localEulerAngles.z > topHeadRotation)
                        {
                            shootMarker.rotation = initRotation;
                        }
                    }
                }
            }

            if (bossControlScheme.shoot.Value() > 0.2)
            {
                if (bulletTimer >= reload)
                {
                    anim.SetTrigger("Shoot");
                    trapSetter.isReadyForAction = false;
                    Invoke("NormalShootDelayed", animShoot.length - shootAnimTimeVariation);
                    bulletTimer = 0;

                    ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.SHOOT, reload));
                }
            }
            //bulletTimer += Time.deltaTime;
        }

        void NormalShootDelayed()
        {
            Shoot();
        }

        void Shoot()
        {
            RaycastHit[] targetLocation;

            laser.SetActive(true);
            float distance = 50.0f;
            float thickness = laser.GetComponent<ParticleSystem>().shape.boxThickness.y; //<-- Desired thickness here.
            LayerMask layers = LayerMask.GetMask("Player") | LayerMask.GetMask("Trap");
            targetLocation = Physics.SphereCastAll(bulletStartingPoint.position,thickness,bulletStartingPoint.right,distance,layers);
            foreach(RaycastHit r in targetLocation)
            {
                ExecuteEvents.Execute<ICharacterEvents>(r.transform.gameObject, null, (x, y) => x.Kill());
            }
            StartCoroutine(DesactivateDelayed(laser,laserDuration));
        }

        IEnumerator DesactivateDelayed(GameObject g, float time)
        {
            yield return new WaitForSeconds(time);
            g.SetActive(false);
        }

        void MeleVerification()
        {
            if (bossControlScheme.melee.Value() > 0.2)
            {
                if (meleTimer >= meleReload && !meleBeingPressed)
                {
                    anim.SetTrigger("Mele");
                    trapSetter.isReadyForAction = false;
                    GameObject tempMele = Instantiate(mele, meleStartingPoint.position, Quaternion.identity);
                    tempMele.GetComponent<Rigidbody>().velocity = new Vector3(0,-meleSpeed * Time.deltaTime,0);
                    meleTimer = 0.0f;
                    meleBeingPressed = true;

                    ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.MELE, meleReload));
                }
            }
            else
            {
                meleBeingPressed = false;
            }
            //meleTimer += Time.deltaTime;
        }
    }
}