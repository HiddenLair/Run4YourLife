using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public class Boss3 : MonoBehaviour
    {
        //Shoot
        public GameObject laser;
        public float laserDuration;
        public float rotationSpeed;
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
            if (!laser.activeInHierarchy)
            {
                ShootVerification();
            }

            MeleVerification();

        }

        void ShootVerification()
        {
            float yInput = bossControlScheme.moveLaserVertical.Value();
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
                if (bulletTimer >= reload)
                {
                    Shoot();
                    bulletTimer = 0;
                }
            }
            bulletTimer += Time.deltaTime;
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
                Destroy(r.collider.gameObject);
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
                if (meleTimer >= meleReload)
                {
                    GameObject tempMele = Instantiate(mele, meleStartingPoint.position, Quaternion.identity);
                    tempMele.GetComponent<Rigidbody>().velocity = new Vector3(0,-meleSpeed * Time.deltaTime,0);
                    meleTimer = 0.0f;
                }
            }
            meleTimer += Time.deltaTime;
        }
    }
}