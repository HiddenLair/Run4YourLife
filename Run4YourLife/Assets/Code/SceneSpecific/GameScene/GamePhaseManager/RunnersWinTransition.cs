using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class RunnersWinTransition : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.RunnersWin; } }

        [SerializeField]
        private PlayableDirector m_positioningCutscene;

        [SerializeField]
        private PlayableDirector m_endCutscene;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private GameObject wall;

        [SerializeField]
        private float xOffsetWall = 2.0f;

        private TimelineAsset timelineAsset;

        private GameObject m_ui;

        private void Awake()
        {
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);
        }

        public override void StartPhase()
        {
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a, b) => a.HideCrossHair());
            StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);
            
            //Unable players input, in order to make them fall to ground from jumps
            List<GameObject> runners = GameplayPlayerManager.Instance.RunnersAlive;
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive); // Alert: You are modifying gameplay player manager list.
            foreach(GameObject g in runners)
            {
                PlayerInstance instance = g.GetComponent<PlayerInstance>(); // Alert: Player instance is a class used to hold playerhandle for the player. It does not control movement
                instance.enabled = false;
            }

            //Stop boss and spawn defensive  wall
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            DeactivateScripts(boss);
            //Note: Make it a transform on the hierarchy and use that position, I would not make it with and offset and the boss's position
            Vector3 wallPos = boss.transform.position;
            wallPos.x += xOffsetWall;
            wall.transform.position = wallPos;
            wall.SetActive(true);

            //Wait for all alive runners to touch ground
            bool done = false;
            while (!done)
            {
                done = true;
                foreach (GameObject g in GameplayPlayerManager.Instance.RunnersAlive)
                {
                    if (!g.GetComponent<CharacterController>().isGrounded)
                    {
                        done = false;
                        break;
                    }
                }
                yield return null;
            }

            //Now, we deactivate all, and start cinemachine
            foreach (GameObject runner in runners)
            {
                DeactivateScripts(runner);
            }           

            //Place runners to revive them and start next timeline
            BindTimelineTracks(m_positioningCutscene,runners, boss);

            m_positioningCutscene.Play();
            yield return new WaitUntil(() => m_positioningCutscene.state != PlayState.Playing); // wait until cutscene has completed

            Unbind(m_positioningCutscene);

            //Revive Players
            List<GameObject> ghosts = GameplayPlayerManager.Instance.GhostsAlive;
            foreach (GameObject g in ghosts)
            {
                GameObject revivedRunner = GameplayPlayerManager.Instance.OnRunnerRevive(g.GetComponent<PlayerHandle>(), g.transform.position);
                DeactivateScripts(revivedRunner);
            }

            //Play end transition
            BindTimelineTracks(m_endCutscene, GameplayPlayerManager.Instance.Runners, boss);

            m_endCutscene.Play();
            yield return new WaitUntil(() => m_endCutscene.state != PlayState.Playing); // wait until cutscene has completed

            Unbind(m_endCutscene);

            GameManager.Instance.EndGame_RunnersWin();
        }

        private void DeactivateScripts(GameObject g)
        {
            foreach (MonoBehaviour mono in g.GetComponents<MonoBehaviour>())
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

        private void BindTimelineTracks(PlayableDirector director, List<GameObject> runners, GameObject boss)
        {
            timelineAsset = (TimelineAsset)director.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (PlayableBinding itm in outputs)
            {
                if (itm.streamName.Contains("Move"))
                {
                    SetTrackBindingByTransform(director,itm, runners, boss);
                }
                else
                {
                    SentTrackBindingByObject(director,itm, runners, boss);
                }

            }
        }

        private void SetTrackBindingByTransform(PlayableDirector director,PlayableBinding itm, List<GameObject> runners, GameObject boss)
        {
            if (itm.streamName.Contains("Player1") && runners.Count > 0)
            {
                director.SetGenericBinding(itm.sourceObject, runners[0].transform);
            }
            else if (itm.streamName.Contains("Player2") && runners.Count > 1)
            {
                director.SetGenericBinding(itm.sourceObject, runners[1].transform);
            }
            else if (itm.streamName.Contains("Player3") && runners.Count > 2)
            {
                director.SetGenericBinding(itm.sourceObject, runners[2].transform);
            }
            else if (itm.streamName.Contains("Boss"))
            {
                director.SetGenericBinding(itm.sourceObject, boss.transform);
            }
        }

        private void SentTrackBindingByObject(PlayableDirector director,PlayableBinding itm, List<GameObject> runners, GameObject boss)
        {
            if (itm.streamName.Contains("Player1") && runners.Count > 0)
            {
                director.SetGenericBinding(itm.sourceObject, runners[0]);
            }
            else if (itm.streamName.Contains("Player2") && runners.Count > 1)
            {
                director.SetGenericBinding(itm.sourceObject, runners[1]);
            }
            else if (itm.streamName.Contains("Player3") && runners.Count > 2)
            {
                director.SetGenericBinding(itm.sourceObject, runners[2]);
            }
            else if (itm.streamName.Contains("Boss"))
            {
                director.SetGenericBinding(itm.sourceObject, boss);
            }
        }

        private void Unbind(PlayableDirector director)
        {
            timelineAsset = (TimelineAsset)director.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (var itm in outputs)
            {
                director.SetGenericBinding(itm.sourceObject, null);
            }
        }

        public override void EndPhase()
        {
           
        }

        public override void DebugEndPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

    }
}
