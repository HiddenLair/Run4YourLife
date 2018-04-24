using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class BuffManager : MonoBehaviour
    {

        #region Inspector

        [SerializeField]
        private GameObject headBumpCollider;

        [SerializeField]
        private GameObject shield;

        #endregion

        #region Variables

        private RunnerState buff;

        #endregion

        #region Buff Management

        public void SubscribeBuff(RunnerState newBuff)
        {
            Destroy(buff);
            buff = newBuff;
        }

        public void UnsubscribeBuff(RunnerState newBuff)
        {
            if(buff == newBuff)
            {
                buff = null;
            }
        }

        public RunnerState GetBuff()
        {
            return buff;
        }

        public void Clear()
        {
            Destroy(buff);
            buff = null;
        }

        #endregion

        #region Shield buff
        public void ActivateShield()
        {
            shield.SetActive(true);
        }

        public void DeactivateShield()
        {
            shield.SetActive(false);
        }
        #endregion
    }
}
