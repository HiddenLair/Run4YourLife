using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.GameManagement
{
    public class TransitionToEasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionToEasyMoveHorizontal; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

        [SerializeField]
        private CinemachineVirtualCamera m_cinemachineVirtualCamera;

        [SerializeField]
        private AudioClip phaseMusic;

        private PlayerSpawner m_playerSpawner;

        private Coroutine m_startPhaseCoroutine;
        private TimelineAsset timelineAsset;
       

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);
        }

        #endregion

        public override void StartPhase()
        {
            if(phaseMusic != null)
            {
                AudioManager.Instance.PlayMusic(phaseMusic);
            }

            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            CameraManager.Instance.TransitionToCamera(m_cinemachineVirtualCamera);

            List<GameObject> runners = m_playerSpawner.ActivateRunners();
            foreach(GameObject runner in runners)
            {
                DeactivateScripts(runner);
            }
            GameObject boss = m_playerSpawner.ActivateBoss();
            DeactivateScripts(boss);

            BindTimelineTracks(runners, boss);
            m_startingCutscene.Play();
            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed
            Unbind();
            foreach(GameObject runner in runners)
            {
                ActivateScripts(runner);
            }
            ActivateScripts(boss);
            GameManager.Instance.ChangeGamePhase(GamePhase.EasyMoveHorizontal);
        }

        private void DeactivateScripts(GameObject g)
        {
            foreach(MonoBehaviour mono in g.GetComponents<MonoBehaviour>())
            {
                mono.enabled = false;
            }
        }

        private void ActivateScripts(GameObject g)
        {
            foreach (MonoBehaviour mono in g.GetComponents<MonoBehaviour>())
            {
                mono.enabled = true;
            }
            Animator anim = g.GetComponent<Animator>();
            Avatar temp = anim.avatar;
            anim.avatar = null;
            anim.avatar = temp;
        }

        private void BindTimelineTracks(List<GameObject> runners, GameObject boss)
        {
            timelineAsset = (TimelineAsset)m_startingCutscene.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (var itm in outputs)
            {
                if (itm.streamName.Contains("Player1") && runners.Count > 0)
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[0]);
                }
                else if (itm.streamName.Contains("Player2") && runners.Count > 1)
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[1]);
                }
                else if (itm.streamName.Contains("Player3") && runners.Count > 2)
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[2]);
                }
                else if (itm.streamName.Contains("Boss"))
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, boss);
                }
            }
        }

        private void Unbind()
        {
            timelineAsset = (TimelineAsset)m_startingCutscene.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (var itm in outputs)
            {
                m_startingCutscene.SetGenericBinding(itm.sourceObject, null);                
            }
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
