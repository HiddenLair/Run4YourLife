using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using Run4YourLife.Player;
using Run4YourLife.UI;

namespace Run4YourLife.GameManagement
{
    public class RunnersWinTransition : TransitionBase
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

        public override void StartPhase()
        {
            StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            //Unable players input, in order to make them fall to ground from jumps
            List<GameObject> runners = new List<GameObject>();
            runners.AddRange(GameplayPlayerManager.Instance.RunnersAlive);
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive);
            List<GameObject> ghosts = GameplayPlayerManager.Instance.GhostsAlive;
            List<GameObject> aliveRunners = GameplayPlayerManager.Instance.RunnersAlive;
            foreach (GameObject g in aliveRunners)
            {
                PlayerInstance instance = g.GetComponent<PlayerInstance>(); // Alert: Player instance is a class used to hold playerhandle for the player. It does not control movement
                instance.enabled = false;
            }
            //We store handles to revive ghost late
            Dictionary<GameObject, PlayerHandle> handles = new Dictionary<GameObject, PlayerHandle>();
            foreach(GameObject g in ghosts)
            {
                PlayerInstance instance = g.GetComponent<PlayerInstance>(); // Alert: Player instance is a class used to hold playerhandle for the player. It does not control movement
                handles[g] = instance.PlayerHandle;
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
                foreach (GameObject g in aliveRunners)
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

            for(int i = 0; i< ghosts.Count;)
            {
                GameObject revivedRunner = GameplayPlayerManager.Instance.OnRunnerRevive(handles[ghosts[i]], ghosts[i].transform.position);
                DeactivateScripts(revivedRunner);
            }

            //Play end transition
            BindTimelineTracks(m_endCutscene, GameplayPlayerManager.Instance.Runners, boss);

            m_endCutscene.Play();
            yield return new WaitUntil(() => m_endCutscene.state != PlayState.Playing); // wait until cutscene has completed

            Unbind(m_endCutscene);

            GameManager.Instance.EndGame_RunnersWin();
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
