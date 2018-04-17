using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Shoot3 : Shoot
    {
        #region Inspector

        [SerializeField]
        private GameObject laser;

        [SerializeField]
        private float maxLaserDistance = 50.0f;

        [SerializeField]
        private float laserDuration = 0.5f;

        #endregion

        public override void ShootByAnim()
        {


            RaycastHit[] targetLocation;

            laser.SetActive(true);
            float thickness = laser.GetComponent<ParticleSystem>().shape.boxThickness.y; //<-- Desired thickness here.
            targetLocation = Physics.SphereCastAll(shootInitZone.position, thickness, shootInitZone.right, maxLaserDistance, Layers.Runner | Layers.Trap);
            foreach (RaycastHit r in targetLocation)
            {
                ExecuteEvents.Execute<ICharacterEvents>(r.transform.gameObject, null, (x, y) => x.Kill());
            }
            // StartCoroutine(DesactivateDelayed(laser, laserDuration));
            StartCoroutine(YieldHelper.WaitForSeconds(g => g.SetActive(false), laser, laserDuration));
        }

        /* IEnumerator DesactivateDelayed(GameObject g, float time)
        {
            yield return new WaitForSeconds(time);
            g.SetActive(false);
        } */
    }
}