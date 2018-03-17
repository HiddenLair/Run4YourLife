using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;
using UnityEngine.EventSystems;

using Run4YourLife.UI;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public class Boss2 : MonoBehaviour
    {
        //Shoot
        public GameObject bullet;
        public float rotationSpeed;
        public float bulletSpeed;
        public AnimationClip animShoot;
        public Transform shootMarker;
        public Transform bulletStartingPoint;
        public float reload;
        public AudioClip shootSFX;

        //Mele
        public GameObject mele;
        public float meleSpeed;
        public float meleReload;
        public AudioClip meleeSFX;

        //Trap Indicator
        public Transform trapIndicator;

        private float bulletTimer;
        private float meleTimer;
        private bool meleBeingPressed = false;
        private const float shootAnimTimeVariation = 0.2f;

        //Head rotation limits
        private const float leftHeadRotation = 35;
        private const float rightHeadRotation = -35;

        private BossControlScheme bossControlScheme;
        private Animator anim;
        private Laser trapSetter;
        private AudioSource audioBoss;
        private GameObject uiManager;

        private void Awake()
        {
            audioBoss = GetComponent<AudioSource>();
            trapSetter = GetComponent<Laser>();
            bossControlScheme = GetComponent<BossControlScheme>();
            bulletTimer = reload;
            meleTimer = meleReload;
            anim = GetComponent<Animator>();

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
            float xInput = bossControlScheme.moveLaserHorizontal.Value();
            if (Mathf.Abs(xInput) > 0.2)
            {
                if (leftHeadRotation >= shootMarker.localEulerAngles.x || 360 + rightHeadRotation <= shootMarker.localEulerAngles.x) {
                    if (xInput < 0)
                    {
                        Quaternion initRotation = shootMarker.rotation;
                        Quaternion temp = initRotation * Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
                        shootMarker.rotation = temp;
                        if(shootMarker.localEulerAngles.x > leftHeadRotation && shootMarker.localEulerAngles.x < 360 + rightHeadRotation)
                        {
                            shootMarker.rotation = initRotation;
                        }
                    }
                    else
                    {
                        Quaternion initRotation = shootMarker.rotation;
                        Quaternion temp = initRotation * Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);
                        shootMarker.rotation = temp;
                        if (shootMarker.localEulerAngles.x < 360 + rightHeadRotation && shootMarker.localEulerAngles.x > leftHeadRotation)
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
            Shoot(bullet);
        }

        void Shoot(GameObject bullet)
        {
            PlaySFX(shootSFX);
            GameObject lastBulletShoot = Instantiate(bullet, bulletStartingPoint.position, bullet.transform.rotation * shootMarker.rotation);
            lastBulletShoot.GetComponent<Rigidbody>().velocity = lastBulletShoot.transform.right * bulletSpeed*Time.deltaTime;
        }

        void MeleVerification()
        {
            if (bossControlScheme.melee.Value() > 0.2)
            {
                if (meleTimer >= meleReload && !meleBeingPressed)
                {
                    PlaySFX(meleeSFX);
                    Vector3 trapPos = trapIndicator.position;
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(trapPos);
                    if (screenPos.x <= Camera.main.pixelWidth / 2)//Left side screen
                    {
                        trapPos.x = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.transform.position.z - trapPos.z)).x;
                        MeleAtack(trapPos, Quaternion.Euler(0, 0, 0));
                    }
                    else//Right side screen
                    {
                        trapPos.x = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z - trapPos.z)).x;
                        MeleAtack(trapPos,Quaternion.Euler(0,180,0));
                    }
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

        void MeleAtack(Vector3 pos,Quaternion rotation)
        {
            GameObject meleInst = Instantiate(mele, pos, rotation);
            meleInst.GetComponent<Rigidbody>().velocity = meleInst.transform.right * meleSpeed *Time.deltaTime;
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
            {
                audioBoss.PlayOneShot(clip);
            }
        }
    }
}

