using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class NextPhaseOnEvent : MonoBehaviour
    {
        [SerializeField]
        private GamePhase m_nextGamePhase;

        public void ExecuteNextPhase()
        {
            GameManager.Instance.EndExecutingPhaseAndStartPhase(m_nextGamePhase);
        }
    }
}
