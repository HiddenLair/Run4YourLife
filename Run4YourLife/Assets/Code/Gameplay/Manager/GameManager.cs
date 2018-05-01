using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;
using Run4YourLife.Player;
using Run4YourLife.Utils;
using System;

namespace Run4YourLife.GameManagement
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public GamePhaseEvent onGamePhaseChanged;

        [SerializeField]
        private SceneTransitionRequest m_loadBossWinsMenu;

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(() => EndExecutingPhaseAndStartPhase(GamePhase.TransitionToEasyMoveHorizontal)));
        }

        public void OnAllRunnersDied()
        {
            m_loadBossWinsMenu.Execute();
        }

        #region Phase Execution

        public GamePhase GamePhase {
            get {
                UnityEngine.Debug.Assert(m_executingGamePhaseManager != null);
                return m_executingGamePhaseManager.GamePhase;
            }
        }

        public Dictionary<GamePhase, GamePhaseManager> gamePhases = new Dictionary<GamePhase, GamePhaseManager>();
        private GamePhaseManager m_executingGamePhaseManager;

        public void RegisterPhase(GamePhase gamePhase, GamePhaseManager gamePhaseManager)
        {
            UnityEngine.Debug.Assert(!gamePhases.ContainsKey(gamePhase), "Error, trying to add multiple gamePhaseManagers with the same gamePhase");
            gamePhases.Add(gamePhase, gamePhaseManager);
        }

        public void EndExecutingPhaseAndStartPhase(GamePhase gamePhase)
        {
            onGamePhaseChanged.Invoke(gamePhase);

            PhaseEnd();
            PhaseStart(gamePhase);
        }

        public void DebugEndExecutingPhaseAndDebugStartPhase(GamePhase gamePhase)
        {
            onGamePhaseChanged.Invoke(gamePhase);

            DebugPhaseEnd();
            DebugPhaseStart(gamePhase);
        }

        public void PhaseStart(GamePhase gamePhase)
        {
            UnityEngine.Debug.Assert(m_executingGamePhaseManager == null);

            m_executingGamePhaseManager = gamePhases[gamePhase];
            m_executingGamePhaseManager.StartPhase();
        }

        public void PhaseEnd()
        {
            if(m_executingGamePhaseManager != null)
            {
                m_executingGamePhaseManager.EndPhase();
                m_executingGamePhaseManager = null;
            }
        }

        public void DebugPhaseStart(GamePhase gamePhase)
        {
            UnityEngine.Debug.Assert(m_executingGamePhaseManager == null);

            m_executingGamePhaseManager = gamePhases[gamePhase];
            m_executingGamePhaseManager.DebugStartPhase();
        }

        public void DebugPhaseEnd()
        {
            if (m_executingGamePhaseManager != null)
            {
                m_executingGamePhaseManager.DebugEndPhase();
                m_executingGamePhaseManager = null;
            }
        }

        #endregion

        #region Debug

        private void Update()
        {
            if(UnityEngine.Debug.isDebugBuild)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad1))
                {
                    DebugEndExecutingPhaseAndDebugStartPhase(GamePhase.EasyMoveHorizontal);
                }
                else if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad2))
                {
                    DebugEndExecutingPhaseAndDebugStartPhase(GamePhase.BossFight);
                }
                else if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad3))
                {
                    DebugEndExecutingPhaseAndDebugStartPhase(GamePhase.HardMoveHorizontal);
                }
                else if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad4))
                {
                    EndExecutingPhaseAndStartPhase(GamePhase.TransitionToBossFight);
                }
                else if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad5))
                {
                    EndExecutingPhaseAndStartPhase(GamePhase.TransitionToHardMoveHorizontal);
                }
            }
        }

        #endregion
    }
}
