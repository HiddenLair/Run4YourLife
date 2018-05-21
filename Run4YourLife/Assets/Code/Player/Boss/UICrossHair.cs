﻿using UnityEngine;
using UnityEngine.UI;

using Run4YourLife.GameManagement;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public interface IUICrossHairEvents : IEventSystemHandler
    {
        void ShowCrossHair();
        void HideCrossHair();
        void AttachCrossHair(CrossHair crossHair);
        void DeatachCrossHair();
    }

    public class UICrossHair : MonoBehaviour, IUICrossHairEvents
    {
        [SerializeField]
        private GameObject m_crossHairUi;

        [SerializeField]
        private Color enabledColor;

        [SerializeField]
        private Color disabledColor;


        private RectTransform m_canvasTransform;
        private RectTransform m_crossHairUiTransform;
        private Camera m_mainCamera;
        private Image m_crossHairImage;


        private CrossHair m_attachedCrossHair;
        private Transform m_attachedCrossHairTransform;

        private void Awake()
        {
            m_canvasTransform = GetComponent<RectTransform>();
            Debug.Assert(m_canvasTransform != null);

            m_crossHairUiTransform = m_crossHairUi.GetComponent<RectTransform>();
            Debug.Assert(m_crossHairUiTransform != null);

            m_crossHairImage = m_crossHairUi.GetComponent<Image>();
            Debug.Assert(m_crossHairImage != null);

            m_mainCamera = CameraManager.Instance.MainCamera;
            Debug.Assert(m_mainCamera != null);
        }

        private void Update()
        {
            if(m_attachedCrossHair != null)
            {
                UpdatePosition();
                UpdateActiveStatus();
            }
        }

        void UpdatePosition()
        {
            Vector2 viewportPosition = m_mainCamera.WorldToViewportPoint(m_attachedCrossHairTransform.position);

            float deltaX = m_canvasTransform.sizeDelta.x;
            float deltaY = m_canvasTransform.sizeDelta.y;

            Vector2 crossHairScreenPosition = new Vector2()
            {
                x = (viewportPosition.x * deltaX) - (deltaX * 0.5f),
                y = (viewportPosition.y * deltaY) - (deltaY * 0.5f)
            };

            m_crossHairUiTransform.anchoredPosition = crossHairScreenPosition;
        }

        void UpdateActiveStatus()
        {
            if (m_attachedCrossHair.IsOperative)
            {
                m_crossHairImage.color = enabledColor;
            }
            else
            {
                m_crossHairImage.color = disabledColor;
            }
        }

        public void ShowCrossHair()
        {
            m_crossHairUi.SetActive(true);
        }

        public void HideCrossHair()
        {
            m_crossHairUi.SetActive(false);
        }

        public void AttachCrossHair(CrossHair crossHair)
        {
            m_attachedCrossHair = crossHair;
            m_attachedCrossHairTransform = crossHair.transform;
        }

        public void DeatachCrossHair()
        {
            m_attachedCrossHair = null;
            m_attachedCrossHairTransform = null;
        }
    }
}
