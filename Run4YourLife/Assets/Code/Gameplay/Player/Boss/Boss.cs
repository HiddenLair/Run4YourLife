using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public class Boss : MonoBehaviour
    {
        //Shoot
        public GameObject bullet1;
        public GameObject bullet2;
        public float rotationSpeed;
        public float bulletSpeed;
        public AnimationClip animShoot;
        public AnimationClip animChargedShoot;
        public float timeToStartChargedShoot;
        public Transform shootMarker;
        public Transform bulletStartingPoint;
        public float reload;


        //Mele
        public GameObject mele;
        public Transform meleZone;
        public float meleReload;

        private float bulletTimer;
        private float meleTimer;
        private float timeToChargedShoot;
        private const float chargedShootAnimTimeVariation = 0.4f;
        private const float shootAnimTimeVariation = 0.2f;
        private bool shootStillAlive = false;//Only for charged shoot
        private bool shootPressed = false;
        private float internalShootTimer = 0;
        private bool explosionShootPressed = false;
        private bool startingChargedShoot = false;

        //Head rotation limits
        private const float topHeadRotation = 55;
        private const float bottomHeadRotation = -7;

        private GameObject lastBulletShoot;
        private BossControlScheme bossControlScheme;
        private Animator anim;
        private Laser trapSetter;

        private void Awake()
        {
            trapSetter = GetComponent<Laser>();
            bossControlScheme = GetComponent<BossControlScheme>();
            bulletTimer = reload;
            meleTimer = meleReload;
            anim = GetComponent<Animator>();
            timeToChargedShoot = animChargedShoot.length -chargedShootAnimTimeVariation;//Substract a little of time, in order to fit more the times
        }

        private void Start()
        {
            bossControlScheme.Active = true;
        }

        void Update()
        {

            if (trapSetter.isReadyForAction || startingChargedShoot)
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
                        Quaternion temp = initRotation * Quaternion.Euler(0, 0, rotationSpeed*Time.deltaTime);
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
                if (!shootStillAlive)
                {
                    if (bulletTimer >= reload)
                    {
                        internalShootTimer += Time.deltaTime;
                        if(internalShootTimer >= timeToStartChargedShoot && !startingChargedShoot)
                        {
                            anim.SetTrigger("ChargedShoot");
                            trapSetter.isReadyForAction = false;
                            startingChargedShoot = true;
                        }
                        if (internalShootTimer >= timeToChargedShoot)
                        {
                            anim.SetTrigger("Break");
                            Shoot(bullet2);
                            shootStillAlive = true;
                            internalShootTimer = 0;
                        }
                    }
                }
                else
                {
                    if (!shootPressed)
                    {
                        Shoot2Detonation();
                        shootStillAlive = false;
                        explosionShootPressed = true;
                    }
                    bulletTimer = 0;
                }
                shootPressed = true;
            }
            else
            {
                shootPressed = false;

                if (internalShootTimer > 0 && internalShootTimer < timeToStartChargedShoot && !explosionShootPressed)
                {
                    anim.SetTrigger("Shoot");
                    trapSetter.isReadyForAction = false;
                    Invoke("NormalShootDelayed", animShoot.length-shootAnimTimeVariation);
                    bulletTimer = 0;
                }
                else if(internalShootTimer > 0)
                {
                    anim.SetTrigger("Break");//Exits the charged shoot anim
                }
                explosionShootPressed = false;
                startingChargedShoot = false;
                internalShootTimer = 0;
            }
            bulletTimer += Time.deltaTime;
        }

        void NormalShootDelayed()
        {
            Shoot(bullet1);
        }

        void Shoot(GameObject bullet)
        {
            lastBulletShoot = Instantiate(bullet, bulletStartingPoint.position, bullet.GetComponent<Transform>().rotation * shootMarker.rotation);
            lastBulletShoot.GetComponent<Rigidbody>().velocity = lastBulletShoot.GetComponent<Transform>().right * bulletSpeed * Time.deltaTime;
            if (lastBulletShoot.GetComponent<ChargedBullet>())
            {
                lastBulletShoot.GetComponent<ChargedBullet>().SetCallback(this);
            }
        }

        public void Shoot2Detonation()
        {
            lastBulletShoot.GetComponent<ChargedBullet>().Explosion();
        }

        void MeleVerification()
        {
            if (bossControlScheme.melee.Value() > 0.2)
            {
                if (meleTimer >= meleReload)
                {
                    anim.SetTrigger("Mele");
                    trapSetter.isReadyForAction = false;
                    var meleInst = Instantiate(mele, meleZone.position, mele.GetComponent<Transform>().rotation);
                    meleInst.transform.SetParent(transform);
                    Destroy(meleInst, 1.0f);
                    meleTimer = 0.0f;
                }
            }
            meleTimer += Time.deltaTime;
        }

        public void SetShootStillAlive(bool value)
        {
            shootStillAlive = value;
        }
    }
}
