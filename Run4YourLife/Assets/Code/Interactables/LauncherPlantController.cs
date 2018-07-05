using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class LauncherPlantController : MonoBehaviour {

        #region Inspector

        [SerializeField]
        private float shootForce;

        [SerializeField]
        private float timeBetweenShoots;

        [SerializeField]
        private Transform shootInitZone;

        [SerializeField]
        private GameObject bullet;

        [SerializeField]
        private int m_nBulletCached;

        #endregion

        private float m_nextLaunchTime;
        private Animator m_animator;
        private GameObjectPool m_gameObjectPool;

        private void Awake()
        {
            m_nextLaunchTime = Time.time + timeBetweenShoots;
            m_animator = GetComponentInChildren<Animator>();
            m_gameObjectPool = GetComponent<GameObjectPool>();
            m_gameObjectPool.Add(bullet, m_nBulletCached);
        }

        void Update () {
		    if(m_nextLaunchTime <= Time.time)
            {
                m_animator.SetTrigger("Shoot");
                Shoot();
                m_nextLaunchTime = Time.time + timeBetweenShoots;
            }
	    }

        private void Shoot()
        {
            GameObject instance = m_gameObjectPool.GetAndPosition(bullet, shootInitZone.position, shootInitZone.rotation);
            instance.SetActive(true);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(instance.transform.up * shootForce);
        }
    }
}

