using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class BounceGhost : BounceableEntityBase
    {
        [SerializeField]
        private float m_bounceForce;

        protected override Vector3 BounceForce
        {
            get
            {
                return m_bounceForce * Vector3.up;
            }
        }

        protected override void BouncedOn()
        {
            //gameObject.SetActive(false);
            ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerReviveRequest(transform.position));
        }
    }
}
