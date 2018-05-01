using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.GameManagement
{
    public class PhaseTransition : MonoBehaviour
    {
        [SerializeField]
        private GamePhase m_nextGamePhase;

        public void Transition()
        {
            GameManager.Instance.EndExecutingPhaseAndStartPhase(m_nextGamePhase);
        }
    }
}

