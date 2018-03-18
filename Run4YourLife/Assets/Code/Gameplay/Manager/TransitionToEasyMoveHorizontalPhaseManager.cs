using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class TransitionToEasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        private GameManager m_gameManager;
        private PlayerSpawner m_playerSpawner;

        #region Initialization

        private void Awake()
        {
            m_gameManager = FindObjectOfType<GameManager>();
            Debug.Assert(m_gameManager != null);

            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            RegisterPhase(GamePhase.TransitionToEasyMoveHorizontal);
        }

        #endregion

        public override void StartPhase()
        {
            m_playerSpawner.InstantiatePlayers();
            GameObject boss = GameObject.FindGameObjectWithTag("Boss"); //TODO make class with all tags and reference that
            Debug.Assert(boss != null);
            //We should make an animation play or somehting cool

            //Once the animation has ended and we are in a stable and easy to manage state,
            //we transition directly to the next state
            StartCoroutine(StartNextFrame());
        }

        IEnumerator StartNextFrame()
        {
            yield return null;
            m_gameManager.EndExecutingPhaseAndStartPhase(GamePhase.EasyMoveHorizontal); // last line of code, maybe execute next frame(?)
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
