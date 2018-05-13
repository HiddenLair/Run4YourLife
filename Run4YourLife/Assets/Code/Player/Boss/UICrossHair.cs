using UnityEngine;
using UnityEngine.UI;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public class UICrossHair : SingletonMonoBehaviour<UICrossHair>
    {
        #region Inspector

        [SerializeField]
        private RectTransform m_mainCanvas;

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
        private Vector2 crossHairScreenPosition;

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
            Vector2 viewportPosition = m_mainCamera.WorldToViewportPoint(crossHair.transform.position);

            float deltaX = m_mainCanvas.sizeDelta.x;
            float deltaY = m_mainCanvas.sizeDelta.y;

            crossHairScreenPosition.x = (viewportPosition.x * deltaX) - (deltaX * 0.5f);
            crossHairScreenPosition.y = (viewportPosition.y * deltaY) - (deltaY * 0.5f);

            crossHairUi.anchoredPosition = crossHairScreenPosition;
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
