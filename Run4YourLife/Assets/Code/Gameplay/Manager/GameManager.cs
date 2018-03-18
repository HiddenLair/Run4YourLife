using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;
using Run4YourLife.Player;
using System;

namespace Run4YourLife.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public GamePhaseEvent onGamePhaseChanged;

        [SerializeField]
        private SceneLoadRequest toMainMenuRequest;

        #region Initialization

        private void Awake()
        {
            PlayerManager playerManager = GetOrCreateDefaultPlayerManagerIfNoneIsAviable();
            Debug.Assert(playerManager != null);
        }

        private PlayerManager GetOrCreateDefaultPlayerManagerIfNoneIsAviable()
        {
            //TODO if no playermanager is found, create default player manager
            //useful for debug opening the scene+
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            if(playerManager == null)
            {
                playerManager = gameObject.AddComponent<PlayerManager>();
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Red,
                    ID = 1,
                    inputDevice = new Input.InputDevice(1),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Orange,
                    ID = 2,
                    inputDevice = new Input.InputDevice(2),
                    IsBoss = true
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Green,
                    ID = 3,
                    inputDevice = new Input.InputDevice(3),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Blue,
                    ID = 4,
                    inputDevice = new Input.InputDevice(4),
                    IsBoss = false
                });
            }
            return playerManager;
        }

        #endregion

        private void Start()
        {
            StartCoroutine(StartNextFrame());
        }

        private IEnumerator StartNextFrame()
        {
            yield return null;
            EndExecutingPhaseAndStartPhase(GamePhase.TransitionToEasyMoveHorizontal);
        }

        private void Update()
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
                EndExecutingPhaseAndStartPhase(GamePhase.TransitionToHardMoveHorizontal);
            }
        }

        public void OnAllRunnersDeath()
        {
            toMainMenuRequest.Execute();
        }

        #region Phase Execution

        public GamePhase GamePhase {
            get {
                Debug.Assert(m_executingGamePhaseManager != null);
                return m_executingGamePhaseManager.GamePhase;
            }
        }

        public Dictionary<GamePhase, GamePhaseManager> gamePhases = new Dictionary<GamePhase, GamePhaseManager>();
        private GamePhaseManager m_executingGamePhaseManager;

        public void RegisterPhase(GamePhase gamePhase, GamePhaseManager gamePhaseManager)
        {
            Debug.Assert(!gamePhases.ContainsKey(gamePhase), "Error, trying to add multiple gamePhaseManagers with the same gamePhase");
            gamePhases.Add(gamePhase, gamePhaseManager);
        }

        public void EndExecutingPhaseAndStartPhase(GamePhase gamePhase)
        {
            PhaseEnd();
            PhaseStart(gamePhase);
            onGamePhaseChanged.Invoke(gamePhase);
        }

        public void DebugEndExecutingPhaseAndDebugStartPhase(GamePhase gamePhase)
        {
            DebugPhaseEnd();
            DebugPhaseStart(gamePhase);
            onGamePhaseChanged.Invoke(gamePhase);
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
    }
}
