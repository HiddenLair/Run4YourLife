using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;

using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    public class BigHeadPowerUp : PowerUp
    {

        #region Inspector

        [SerializeField]
        private float headModificatorIncrease = 2.0f;

        [SerializeField]
        private float jumpOverMeValue = 6.0f;

        [SerializeField]
        private float time = 10.0f;

        #endregion

        #region Variables

        private float initialJumpOverMeValue = 0;

        #endregion

        public override void Effect(GameObject g)
        {
            HeadModifier tempHeadScript = g.GetComponent<HeadModifier>();
            tempHeadScript.IncreaseHead(headModificatorIncrease);

            initialJumpOverMeValue = tempHeadScript.GetJumpOverMeValue();
            tempHeadScript.SetJumpOverMeValue(jumpOverMeValue);

            // CoroutineManager.GetInstance().StartCoroutine(DeactivateAfterTime(g));
            CoroutineManager.GetInstance().StartCoroutine(YieldHelper.WaitForSeconds(Deactivate, g, time));
        }

        private void Deactivate(GameObject g)
        {
            HeadModifier tempHeadScript = g.GetComponent<HeadModifier>();
            tempHeadScript.DecreaseHead(headModificatorIncrease);

            tempHeadScript.SetJumpOverMeValue(initialJumpOverMeValue);
        }

        /* IEnumerator DeactivateAfterTime(GameObject g)
        {
            yield return new WaitForSeconds(time);
            HeadModifier tempHeadScript = g.GetComponent<HeadModifier>();
            tempHeadScript.DecreaseHead(headModificatorIncrease);
            
            tempHeadScript.SetJumpOverMeValue(initialJumpOverMeValue);
        } */
    }
}