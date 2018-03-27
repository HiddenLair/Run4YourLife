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
        GameObject crossHair = null;

        [SerializeField]
        private Color enabledColor;

        [SerializeField]
        private Color disabledColor;
        #endregion

        #region Variables

        Image img;

        #endregion

        private void Awake()
        {
            img = GetComponent<Image>();
        }

        private void Update()
        {
            if (crossHair != null)
            {
                img.enabled = true;

                Move();

                CheckStatus();
            }
            else
            {
                img.enabled = false;
            }
        }

        void Move()
        {
            transform.position = Camera.main.WorldToScreenPoint(crossHair.transform.position);
        }

        void CheckStatus()
        {
            if (crossHair.GetComponent<CrossHair>().GetActive())
            {
                img.color = enabledColor;
            }
            else
            {
                img.color = disabledColor;
            }
        }

        public void SubscribeWorldCrossHair(GameObject crossHair)
        {
            this.crossHair = crossHair;
        }

        public void UnsubscribeWorldCrossHair(GameObject crossHair)
        {
         if(this.crossHair == crossHair)
            {
                crossHair = null;
            }   
        }
    }
}
