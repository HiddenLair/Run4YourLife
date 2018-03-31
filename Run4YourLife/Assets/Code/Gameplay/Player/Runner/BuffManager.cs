using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class BuffManager : MonoBehaviour
    {

        #region Inspector

        [SerializeField]
        private GameObject head;

        [SerializeField]
        private GameObject headBumpCollider;

        [SerializeField]
        private GameObject shield;

        #endregion

        public Vector3 GetHeadScale()
        {
            return head.transform.localScale;
        }

        public void IncreaseHead(float value)
        {
            Vector3 temp = new Vector3(value, value, value);
            head.transform.localScale += temp;
            headBumpCollider.transform.localScale += temp;
        }

        public void DecreaseHead(float value)
        {
            Vector3 temp = new Vector3(value, value, value);
            head.transform.localScale -= temp;
            headBumpCollider.transform.localScale -= temp;
        }

        public void IncreaseHeadPercentual(float percent)
        {
            Vector3 tempMesh = head.transform.localScale * percent;
            Vector3 tempCollider = headBumpCollider.transform.localScale * percent;
            head.transform.localScale += tempMesh;
            headBumpCollider.transform.localScale += tempCollider;
        }

        public void DecreaseHeadPercentual(float percent)
        {
            Vector3 tempMesh = head.transform.localScale * percent;
            Vector3 tempCollider = headBumpCollider.transform.localScale * percent;
            head.transform.localScale -= tempMesh;
            headBumpCollider.transform.localScale -= tempCollider;
        }

        public void ActivateShield()
        {
            shield.SetActive(true);
        }

        public void DeactivateShield()
        {
            shield.SetActive(false);
        }
    }
}
