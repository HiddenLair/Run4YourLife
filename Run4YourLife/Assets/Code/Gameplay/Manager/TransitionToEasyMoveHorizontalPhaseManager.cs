using System;
using System.Collections;
using UnityEngine;

using UnityEngine.Playables;

namespace Run4YourLife.GameManagement
{
    public class TransitionToEasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        [SerializeField]
        private PlayableDirector m_startingCutscene;

        private PlayerSpawner m_playerSpawner;

        private Coroutine m_startPhaseCoroutine;

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            RegisterPhase(GamePhase.TransitionToEasyMoveHorizontal);
        }

        #endregion

        public override void StartPhase()
        {
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            m_playerSpawner.ActivateRunners();
            //m_startingCutscene.Play();
            //yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed
            m_playerSpawner.ActivateBoss();
            yield return null;
            GameManager.Instance.EndExecutingPhaseAndStartPhase(GamePhase.EasyMoveHorizontal);
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
            StopCoroutine(m_startPhaseCoroutine);
            m_startingCutscene.Stop();
            m_startPhaseCoroutine = null;
        }
    }
}
