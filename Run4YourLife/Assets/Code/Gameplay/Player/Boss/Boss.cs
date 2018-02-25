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
        public GameObject shoot;
        public GameObject shoot2;
        public float rotationSpeed;
        public float bulletSpeed;
        public Transform bulletStartingPoint;
        public float reload;

        //Mele
        public GameObject mele;
        public Transform meleZone;
        public float meleReload;

        private float timer;
        private float meleTimer;
        private int shootMode = 0;
        private bool shootStillAlive = false;//Only for second shoot

        private BossControlScheme bossControlScheme;

        private void Awake()
        {
            bossControlScheme = GetComponent<BossControlScheme>();
            timer = reload;
            meleTimer = meleReload;

        }

        private void Update()
        {
            switch (shootMode)
            {
                case 0:
                    {
                        Shoot1();
                    }
                    break;
                case 1:
                    {
                        Shoot2();
                    }
                    break;
            }

            if (bossControlScheme.melee.Value() > 0.2)
            {
                if (meleTimer >= meleReload)
                {
                    var meleInst = Instantiate(mele, meleZone.position, mele.GetComponent<Transform>().rotation);
                    Destroy(meleInst, 1.0f);
                    meleTimer = 0.0f;
                }
            }
            meleTimer += Time.deltaTime;
        }

        void Shoot1()
        {
            float yInput = bossControlScheme.moveLaserVertical.Value();
            if (Mathf.Abs(yInput) > 0.2)
            {
                if (yInput < 0)
                {
                    Quaternion temp = transform.rotation * Quaternion.Euler(0, 0, rotationSpeed);
                    transform.rotation = temp;
                }
                else
                {
                    Quaternion temp = transform.rotation * Quaternion.Euler(0, 0, -rotationSpeed);
                    transform.rotation = temp;
                }

            }

            if (bossControlScheme.shoot.Value() > 0.2)
            {
                if (timer >= reload)
                {
                    var bulletInst = Instantiate(shoot, bulletStartingPoint.position, shoot.GetComponent<Transform>().rotation * transform.rotation);
                    bulletInst.GetComponent<Rigidbody>().velocity = bulletInst.GetComponent<Transform>().up * bulletSpeed;
                    timer = 0;
                }
            }
            timer += Time.deltaTime;
        }

        void Shoot2()
        {
            if (bossControlScheme.shoot.Value() > 0.2)
            {
                if (!shootStillAlive)
                {
                    var bulletInst = Instantiate(shoot2, bulletStartingPoint.position, shoot2.GetComponent<Transform>().rotation * transform.rotation);
                    bulletInst.GetComponent<Rigidbody>().velocity = new Vector3(-bulletSpeed, 0, 0);
                    bulletInst.GetComponent<MovingBullet>().SetCallback(gameObject);
                    shootStillAlive = true;
                }
            }
        }

        public void SetShootStillAlive()
        {
            shootStillAlive = false;
        }
    }

}