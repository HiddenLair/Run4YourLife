using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToBossFightPhaseManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        #endregion

        private GameplayPlayerManager m_gameplayPlayerManager;
        private GameManager gameManager;

        #region Initialization

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            Debug.Assert(gameManager != null);
            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);

            RegisterPhase(GamePhase.TransitionToBossFight);
        }

        #endregion

        public override void StartPhase()
        {           
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);
           
            gameManager.EndExecutingPhaseAndStartPhase(GamePhase.BossFight);
        }

        public override void EndPhase()
        {
            
        }

     

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            
        }
    }
}
