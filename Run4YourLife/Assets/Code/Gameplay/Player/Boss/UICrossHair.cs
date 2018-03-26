using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.Player
{
    public class UICrossHair : MonoBehaviour
    {

        #region Inspector
        [SerializeField]
        GameObject crossHair;

        [SerializeField]
        private Color enabledColor;

        [SerializeField]
        private Color disabledColor;
        #endregion

        private void Update()
        {
            Move();

            CheckStatus();
        }

        void Move()
        {
            transform.position = Camera.main.WorldToScreenPoint(crossHair.transform.position);
        }

        void CheckStatus()
        {
            if (crossHair.GetComponent<CrossHair>().GetActive())
            {
                GetComponent<Image>().color = enabledColor;
            }
            else
            {
                GetComponent<Image>().color = disabledColor;
            }
        }
    }
}
