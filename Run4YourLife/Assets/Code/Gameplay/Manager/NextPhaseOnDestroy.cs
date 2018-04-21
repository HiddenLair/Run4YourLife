using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class NextPhaseOnDestroy : MonoBehaviour
    {
        [SerializeField]
        private GamePhase m_nextGamePhase;

        private void OnDestroy()
        {
            GameManager.Instance.EndExecutingPhaseAndStartPhase(m_nextGamePhase);
        }
    }
}
