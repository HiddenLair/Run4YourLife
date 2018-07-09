using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;

namespace Run4YourLife.UI
{
    public enum ActionType
    {
        MELEE, SHOOT,
        A, B, X, Y
    }

    public interface IUICrossHairEvents : IEventSystemHandler
    {
        void OnActionUsed(ActionType actionType, float time);
    }

    public class UICrossHair : SingletonMonoBehaviour<UICrossHair>, IUICrossHairEvents
    {
        #region Editor

        [SerializeField]
        private GameObject m_crossHairUi;

        [SerializeField]
        private UIBossActionController m_melee;

        [SerializeField]
        private UIBossActionController m_shoot;

        [SerializeField]
        private UIBossActionController m_A;

        [SerializeField]
        private UIBossActionController m_B;

        [SerializeField]
        private UIBossActionController m_X;

        [SerializeField]
        private UIBossActionController m_Y;

        #endregion

        private Camera m_mainCamera;
        private RectTransform m_canvasTransform;
        private RectTransform m_crossHairUiTransform;
        private CanvasGroup crossHairCanvasGroup;
        private UIBossActionController[] m_actionControllers;

        private void Awake()
        {
            m_mainCamera = CameraManager.Instance.MainCamera;
            Debug.Assert(m_mainCamera != null);

            m_canvasTransform = GetComponent<RectTransform>();
            Debug.Assert(m_canvasTransform != null);

            m_crossHairUiTransform = m_crossHairUi.GetComponent<RectTransform>();
            Debug.Assert(m_crossHairUiTransform != null);

            crossHairCanvasGroup = m_crossHairUi.GetComponent<CanvasGroup>();
            Debug.Assert(crossHairCanvasGroup != null);

            m_actionControllers = new UIBossActionController[Enum.GetNames(typeof(ActionType)).Length];
            m_actionControllers[(int)ActionType.MELEE] = m_melee;
            m_actionControllers[(int)ActionType.SHOOT] = m_shoot;
            m_actionControllers[(int)ActionType.A] = m_A;
            m_actionControllers[(int)ActionType.B] = m_B;
            m_actionControllers[(int)ActionType.X] = m_X;
            m_actionControllers[(int)ActionType.Y] = m_Y;

            GameManager.Instance.onGamePhaseChanged.AddListener(OnGamePhaseChanged);
        }

        private void OnGamePhaseChanged(GamePhase gamePhase)
        {
            foreach(UIBossActionController bossActionController in m_actionControllers)
            {
                bossActionController.Reset();
            }

            // Tutorial
            if(gamePhase == GamePhase.EasyMoveHorizontal || gamePhase == GamePhase.BossFight || gamePhase == GamePhase.HardMoveHorizontal)
            {
                ShowCrossHair();
            }
            else
            {
                HideCrossHair();
            }
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

        public void OnActionUsed(ActionType actionType, float time)
        {
            m_actionControllers[(int)actionType].Use(time);
        }

        private void ShowCrossHair()
        {
            crossHairCanvasGroup.alpha = 1.0f;
        }

        private void HideCrossHair()
        {
            crossHairCanvasGroup.alpha = 0.0f;
        }
    }
}