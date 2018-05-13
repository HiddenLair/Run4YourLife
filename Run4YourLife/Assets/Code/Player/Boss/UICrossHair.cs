using UnityEngine;
using UnityEngine.UI;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public class UICrossHair : SingletonMonoBehaviour<UICrossHair>
    {
        #region Inspector

        [SerializeField]
        private RectTransform crossHairUi;

        [SerializeField]
        private Color enabledColor;

        [SerializeField]
        private Color disabledColor;

        #endregion

        #region Variables

        private GameObject crossHair = null;
        private Image crosshairImage;
        private Camera m_mainCamera;

        #endregion

        private void Awake()
        {
            crosshairImage = crossHairUi.GetComponent<Image>();
            Debug.Assert(crosshairImage != null);

            m_mainCamera = CameraManager.Instance.MainCamera;
            Debug.Assert(m_mainCamera != null);
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
            crossHairUi.anchoredPosition = m_mainCamera.WorldToScreenPoint(crossHair.transform.position);
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
