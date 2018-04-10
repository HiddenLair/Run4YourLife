using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.OptionsMenu
{
    class GraphicsSwitch : MonoBehaviour, IMoveHandler
    {
        #region Public Variables
        public TextMeshProUGUI graphicsText;
        #endregion

        public void Start()
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
    }
}
