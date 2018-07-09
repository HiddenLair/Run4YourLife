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
    public interface IGameplayEvents : IEventSystemHandler
    {
        void ChangeGamePhase(GamePhase gamePhase);
        void DebugChangePhase(GamePhase gamePhase);
        void EndGame_RunnersWin();
        void EndGame_BossWin();
    }

    public class GameManager : SingletonMonoBehaviour<GameManager>, IGameplayEvents
    {
        [SerializeField]
        GamePhase StartGamePhase;

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
            StartCoroutine(YieldHelper.SkipFrame(() => ChangeGamePhase(StartGamePhase)));
        }

        #region Phase Execution

        public void ChangeGamePhase(GamePhase gamePhase)
        {
            onGamePhaseChanged.Invoke(gamePhase);

            PhaseEnd();
            PhaseStart(gamePhase);
        }

        public void DebugChangePhase(GamePhase gamePhase)
        {
            onGamePhaseChanged.Invoke(gamePhase);

            DebugPhaseEnd();
            DebugPhaseStart(gamePhase);
        }

        private void PhaseStart(GamePhase gamePhase)
        {
            Debug.Assert(m_executingGamePhaseManager == null);

            m_executingGamePhaseManager = gamePhases[gamePhase];
            m_executingGamePhaseManager.StartPhase();
        }

        private void PhaseEnd()
        {
            if(m_executingGamePhaseManager != null)
            {
                m_executingGamePhaseManager.EndPhase();
                m_executingGamePhaseManager = null;
            }
        }

        private void DebugPhaseStart(GamePhase gamePhase)
        {
            Debug.Assert(m_executingGamePhaseManager == null);

            m_executingGamePhaseManager = gamePhases[gamePhase];
            m_executingGamePhaseManager.DebugStartPhase();
        }

        private void DebugPhaseEnd()
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
                    DebugChangePhase(GamePhase.EasyMoveHorizontal);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad2))
                {
                    DebugChangePhase(GamePhase.BossFight);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad3))
                {
                    DebugChangePhase(GamePhase.HardMoveHorizontal);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad4))
                {
                    ChangeGamePhase(GamePhase.TransitionPhase1End);
                }
                else if(Input.GetKeyDown(KeyCode.Keypad5))
                {
                    ChangeGamePhase(GamePhase.TransitionPhase3Start);
                }
            }
        }

        #endregion
        
        public void EndGame_RunnersWin()
        {
            StartCoroutine(YieldHelper.SkipFrame(() => m_loadRunnersWinMenu.Execute()));
        }

        public void EndGame_BossWin()
        {
            StartCoroutine(YieldHelper.SkipFrame(() => m_loadBossWinMenu.Execute()));
        }
    }
}
