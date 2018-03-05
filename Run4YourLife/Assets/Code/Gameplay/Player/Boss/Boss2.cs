using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public class Boss2 : MonoBehaviour
    {
        //Shoot
        public GameObject bullet;
        public float rotationSpeed;
        public float bulletSpeed;
        public Transform shootMarker;
        public Transform bulletStartingPoint;
        public float reload;

        //Mele
        public GameObject mele;
        public float meleSpeed;
        public float meleReload;

        //Trap Indicator
        public Transform trapIndicator;

        private float bulletTimer;
        private float meleTimer;

        private BossControlScheme bossControlScheme;
        private Animator anim;

        private void Awake()
        {
            bossControlScheme = GetComponent<BossControlScheme>();
            bulletTimer = reload;
            meleTimer = meleReload;
            anim = GetComponent<Animator>();
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
            float xInput = bossControlScheme.moveLaserHorizontal.Value();
            if (Mathf.Abs(xInput) > 0.2)
            {
                if (xInput < 0)
                {
                    Quaternion temp = shootMarker.rotation * Quaternion.Euler(0, rotationSpeed, 0);
                    shootMarker.rotation = temp;
                }
                else
                {
                    Quaternion temp = shootMarker.rotation * Quaternion.Euler(0, -rotationSpeed, 0);
                    shootMarker.rotation = temp;
                }
            }

            if (bossControlScheme.shoot.Value() > 0.2)
            {
                if (bulletTimer >= reload)
                {
                    anim.SetTrigger("Shoot");
                    Shoot(bullet);
                    bulletTimer = 0;
                }         
            }
            bulletTimer += Time.deltaTime;
        }

        void Shoot(GameObject bullet)
        {
            GameObject lastBulletShoot = Instantiate(bullet, bulletStartingPoint.position, bullet.transform.rotation * shootMarker.rotation);
            lastBulletShoot.GetComponent<Rigidbody>().velocity = lastBulletShoot.transform.right * bulletSpeed;
        }

        void MeleVerification()
        {
            if (bossControlScheme.melee.Value() > 0.2)
            {
                if (meleTimer >= meleReload)
                {
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
                }
            }
            meleTimer += Time.deltaTime;
        }

        void MeleAtack(Vector3 pos,Quaternion rotation)
        {
            GameObject meleInst = Instantiate(mele, pos, rotation);
            meleInst.GetComponent<Rigidbody>().velocity = meleInst.transform.right * meleSpeed;
        }
    }
}

