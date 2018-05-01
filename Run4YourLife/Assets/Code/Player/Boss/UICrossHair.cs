using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.Player
{
    public class UICrossHair : SingletonMonoBehaviour<UICrossHair>
    {
        #region Inspector

        [SerializeField]
        private Transform crossHairUi;

        [SerializeField]
        private Color enabledColor;

        [SerializeField]
        private Color disabledColor;

        #endregion

        #region Variables

        private GameObject crossHair = null;
        private Image crosshairImage;

        #endregion

        private void Awake()
        {
            crosshairImage = crossHairUi.GetComponent<Image>();
            UnityEngine.Debug.Assert(crosshairImage != null);
        }

        private void Update()
        {
            if (crossHair != null)
            {
                crosshairImage.enabled = true;

                Move();

                CheckStatus();
            }
            else
            {
                crosshairImage.enabled = false;
            }
        }

        void Move()
        {
            crossHairUi.position = Camera.main.WorldToScreenPoint(crossHair.transform.position);
        }

        void CheckStatus()
        {
            if (crossHair.GetComponent<CrossHair>().GetActive())
            {
                crosshairImage.color = enabledColor;
            }
            else
            {
                crosshairImage.color = disabledColor;
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
