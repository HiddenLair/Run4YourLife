using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class NextPhaseOnDestroy : MonoBehaviour
    {

        [SerializeField]
        private GamePhase m_nextGamePhase;

        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void OnDestroy()
        {
            gameManager.EndExecutingPhaseAndStartPhase(m_nextGamePhase);
        }
    }
}
