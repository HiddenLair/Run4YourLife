using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public interface IUICrossHairEvents : IEventSystemHandler
    {
        void ShowCrossHair();
        void HideCrossHair();
        void UpdatePosition(Vector3 position);
    }

    public class UICrossHair : SingletonMonoBehaviour<UICrossHair>, IUICrossHairEvents
    {
        [SerializeField]
        private GameObject m_crossHairUi;

        private Camera m_mainCamera;
        private RectTransform m_canvasTransform;
        private RectTransform m_crossHairUiTransform;

        private void Awake()
        {
            m_mainCamera = CameraManager.Instance.MainCamera;
            Debug.Assert(m_mainCamera != null);

            m_canvasTransform = GetComponent<RectTransform>();
            Debug.Assert(m_canvasTransform != null);

            m_crossHairUiTransform = m_crossHairUi.GetComponent<RectTransform>();
            Debug.Assert(m_crossHairUiTransform != null);
        }

        public void ShowCrossHair()
        {
            m_crossHairUi.SetActive(true);
        }

        public void HideCrossHair()
        {
            m_crossHairUi.SetActive(false);
        }

        public void UpdatePosition(Vector3 position)
        {
            Vector2 viewportPosition = m_mainCamera.WorldToViewportPoint(position);

            float deltaX = m_canvasTransform.sizeDelta.x;
            float deltaY = m_canvasTransform.sizeDelta.y;

            Vector2 crossHairScreenPosition = new Vector2()
            {
                x = (viewportPosition.x * deltaX) - (deltaX * 0.5f),
                y = (viewportPosition.y * deltaY) - (deltaY * 0.5f)
            };

            m_crossHairUiTransform.anchoredPosition = crossHairScreenPosition;
        }
    }
}