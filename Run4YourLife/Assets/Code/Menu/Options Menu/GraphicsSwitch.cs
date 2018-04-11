using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.OptionsMenu
{
    class GraphicsSwitch : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        #region Public Variables
        public TextMeshProUGUI graphicsText;
        public GameObject leftSwitch;
        public GameObject rightSwitch;
        #endregion

        public void Awake()
        {
            UpdateUI();
        }

        public void OnMove(AxisEventData eventData)
        {
            if(eventData.moveDir == MoveDirection.Right)
            {
                QualitySettings.IncreaseLevel();
                UpdateUI();
            }
            else if(eventData.moveDir == MoveDirection.Left)
            {
                QualitySettings.DecreaseLevel();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            graphicsText.SetText(QualitySettings.names[QualitySettings.GetQualityLevel()]);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (leftSwitch != null)
            {
                leftSwitch.SetActive(true);
            }

            if (rightSwitch != null)
            {
                rightSwitch.SetActive(true);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (leftSwitch != null)
            {
                leftSwitch.SetActive(false);
            }

            if (rightSwitch != null)
            {
                rightSwitch.SetActive(false);
            }
        }
    }
}
