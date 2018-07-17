using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player.Boss
{
    public class BossRegularBulletInvisibleNotifier : MonoBehaviour {

        private BossRegularBulletController m_bossRegularBulletController;

        private void Awake()
        {
            m_bossRegularBulletController = transform.parent.GetComponent<BossRegularBulletController>();
            Debug.Assert(m_bossRegularBulletController != null);
        }

        private void OnBecameInvisible()
        {
            m_bossRegularBulletController.RegularBulletBecameInvisible();
        }
    }
}