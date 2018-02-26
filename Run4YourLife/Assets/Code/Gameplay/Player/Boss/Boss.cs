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
        public float timeToChargedShoot;
        public Transform shootMarker;
        public Transform bulletStartingPoint;
        public float reload;

        //Mele
        public GameObject mele;
        public Transform meleZone;
        public float meleReload;

        private float bulletTimer;
        private float meleTimer;
        private bool shootStillAlive = false;//Only for charged shoot
        private bool shootPressed = false;
        private float internalShootTimer = 0;
        private bool explosionShootPressed = false;

        private GameObject lastBulletShoot;
        private BossControlScheme bossControlScheme;

        private void Awake()
        {
            bossControlScheme = GetComponent<BossControlScheme>();
            bulletTimer = reload;
            meleTimer = meleReload;
        }

        private void Start()
        {
            bossControlScheme.Active = true;
        }

        void Update()
        {

            ShootVerification();

            MeleVerification();

        }

        void ShootVerification()
        {
            float yInput = bossControlScheme.moveLaserVertical.Value();
            Debug.Log(yInput);
            if (Mathf.Abs(yInput) > 0.2)
            {
                if (yInput < 0)
                {
                    Quaternion temp = shootMarker.rotation * Quaternion.Euler(0, 0, rotationSpeed);
                    shootMarker.rotation = temp;
                }
                else
                {
                    Quaternion temp = shootMarker.rotation * Quaternion.Euler(0, 0, -rotationSpeed);
                    shootMarker.rotation = temp;
                }
            }

            if (bossControlScheme.shoot.Value() > 0.2)
            {
                if (!shootStillAlive)
                {
                    if (bulletTimer >= reload)
                    {
                        internalShootTimer += Time.deltaTime;
                        if (internalShootTimer > timeToChargedShoot)
                        {
                            Shoot(bullet2);
                            shootStillAlive = true;
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
                }
                shootPressed = true;
            }
            else
            {
                shootPressed = false;

                if (internalShootTimer > 0 && internalShootTimer < timeToChargedShoot && !explosionShootPressed)
                {
                    Shoot(bullet1);
                    bulletTimer = 0;
                }

                explosionShootPressed = false;
                internalShootTimer = 0;
            }
            bulletTimer += Time.deltaTime;
        }

        void Shoot(GameObject bullet)
        {
            lastBulletShoot = Instantiate(bullet, bulletStartingPoint.position, bullet.GetComponent<Transform>().rotation * shootMarker.rotation);
            lastBulletShoot.GetComponent<Rigidbody>().velocity = lastBulletShoot.GetComponent<Transform>().right * bulletSpeed;
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
