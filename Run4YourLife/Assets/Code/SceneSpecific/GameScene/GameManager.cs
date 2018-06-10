using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;
using Run4YourLife.Player;
using Run4YourLife.Utils;
using System;
using Run4YourLife.Debugging;

namespace Run4YourLife.GameManagement
{
    public enum GameEndResult 
    {
        RunnersWin,
        BossWin
    }

    public interface IGameplayEvents : IEventSystemHandler
    {
        void OnGameEnded(GameEndResult gameEndResult);
    }

    public class GameManager : SingletonMonoBehaviour<GameManager>, IGameplayEvents
    {
        public GamePhaseEvent onGamePhaseChanged;

        [SerializeField]
        private SceneTransitionRequest m_loadBossWinMenu;

        [SerializeField]
        private SceneTransitionRequest m_loadRunnersWinMenu;

        public GamePhase GamePhase { get { return m_executingGamePhaseManager.GamePhase; } }

        private Dictionary<GamePhase, GamePhaseManager> gamePhases = new Dictionary<GamePhase, GamePhaseManager>();
        private GamePhaseManager m_executingGamePhaseManager;

        private void Awake()
        {
            foreach (GamePhaseManager gamePhaseManager in FindObjectsOfType<GamePhaseManager>())
            {
                Debug.Assert(!gamePhases.ContainsKey(gamePhaseManager.GamePhase), "Error, trying to add multiple gamePhaseManagers with the same gamePhase");
                gamePhases.Add(gamePhaseManager.GamePhase, gamePhaseManager);
            }
        }

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(() => EndExecutingPhaseAndStartPhase(GamePhase.TransitionToEasyMoveHorizontal)));
        }

        #region Phase Execution

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
            Debug.Assert(m_executingGamePhaseManager == null);

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
            Debug.Assert(m_executingGamePhaseManager == null);

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

        private void OnApplicationQuit()
        {
            MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }

        #region Debug

        private void Update()
        {
            if(DebugSystemManagerHelper.DebuggingToolsEnabled())
            {
                if(Input.GetKeyDown(KeyCode.Keypad1))
                {
                    DebugEndExecutingPhaseAndDebugStartPhase(GamePhase.EasyMoveHorizontal);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad2))
                {
                    DebugEndExecutingPhaseAndDebugStartPhase(GamePhase.BossFight);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad3))
                {
                    DebugEndExecutingPhaseAndDebugStartPhase(GamePhase.HardMoveHorizontal);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad4))
                {
                    EndExecutingPhaseAndStartPhase(GamePhase.TransitionToBossFight);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad5))
                {
                    EndExecutingPhaseAndStartPhase(GamePhase.TransitionToHardMoveHorizontal);
                }
            }
        }

        

        #endregion
        
        public void OnGameEnded(GameEndResult gameEndResult)
        {
            switch(gameEndResult)
            {
                case GameEndResult.BossWin:
                    StartCoroutine(YieldHelper.SkipFrame(() => m_loadBossWinMenu.Execute()));
                    break;
                case GameEndResult.RunnersWin:
                    StartCoroutine(YieldHelper.SkipFrame(() => m_loadRunnersWinMenu.Execute()));
                    break;
            }
        }

        public void OnFinalRockDestroyed()
        {
            ExecuteEvents.Execute<IGameplayEvents>(gameObject, null, (a, b) => a.OnGameEnded(GameEndResult.RunnersWin));
        }
    }
}
