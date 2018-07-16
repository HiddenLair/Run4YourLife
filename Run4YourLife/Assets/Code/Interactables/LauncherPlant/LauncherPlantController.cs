using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;

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

        [SerializeField]
        private AudioClip m_launchAudioClip;

        #endregion

        private float m_nextLaunchTime;
        private Animator m_animator;
        private GameObjectPool m_gameObjectPool;

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
            m_gameObjectPool = GetComponent<GameObjectPool>();
            m_gameObjectPool.Request(bullet, m_nBulletCached);
        }

        private void OnEnable()
        {
            m_nextLaunchTime = Time.time + timeBetweenShoots;
        }

        void Update () {
		    if(m_nextLaunchTime <= Time.time)
            {
                Shoot();
                m_nextLaunchTime += timeBetweenShoots;
            }
	    }

        private void Shoot()
        {
            AudioManager.Instance.PlaySFX(m_launchAudioClip);
            GameObject instance = m_gameObjectPool.GetAndPosition(bullet, shootInitZone.position, shootInitZone.rotation);
            instance.SetActive(true);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(instance.transform.up * shootForce);

            m_animator.SetTrigger("Shoot");
        }
    }
}

