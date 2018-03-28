using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class HeadModifier : MonoBehaviour
    {

        #region Inspector

        [SerializeField]
        private GameObject head;

        [SerializeField]
        private GameObject headBumpCollider;

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

        public void SetJumpOverMeValue(float value)
        {
            headBumpCollider.GetComponent<PlayerJumpOver>().SetBounceForce(value);
        }

        public float GetJumpOverMeValue()
        {
            return headBumpCollider.GetComponent<PlayerJumpOver>().GetBounceForce();
        }
    }
}
