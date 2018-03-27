using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class ShiledPowerUp : PowerUp
    {

        #region Inspector

        [SerializeField]
        private GameObject shield;

        [SerializeField]
        private float time;

        #endregion

        #region Variables

        GameObject instanceShield = null;

        #endregion

        public override void Effect(GameObject g)
        {
            instanceShield = Instantiate(shield,g.transform.position,g.transform.rotation);
            instanceShield.transform.SetParent(g.transform);
            StartCoroutine(DeactivateAfterTime());
        }

        IEnumerator DeactivateAfterTime()
        {
            yield return new WaitForSeconds(time);
            Destroy(instanceShield);
        }
    }
}
