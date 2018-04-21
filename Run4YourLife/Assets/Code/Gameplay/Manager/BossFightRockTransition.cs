using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class BossFightRockTransition : GamePhaseManager
    {

        #region Inspector
        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private GameObject bossRockToMove;

        [SerializeField]
        private GameObject cameraLookAtThis;

        [SerializeField]
        private float time;
        #endregion

        private void Awake()
        {
            RegisterPhase(GamePhase.BossFightRockTransition);
        }

        public override void StartPhase()
        {
            m_virtualCamera.LookAt=cameraLookAtThis.transform;
            m_virtualCamera.Follow = cameraLookAtThis.transform;
            m_virtualCamera.gameObject.SetActive(true);

            List<GameObject> runners = GameplayPlayerManager.Instance.Runners;

            foreach (GameObject g in runners)
            {
                RunnerCharacterController tempController = g.GetComponent<RunnerCharacterController>();
                tempController.SetLimitScreenRight(true);
                tempController.SetLimitScreenLeft(true);
                tempController.SetCheckOutScreen(true);
            }

            Vector3 bossPos = bossRockToMove.transform.position;
            StartCoroutine(SlowMove(bossRockToMove, new Vector3(bossPos.x, 0, 0), time));
        }

        IEnumerator SlowMove(GameObject g, Vector3 destination, float time)
        {
            float endTime = Time.time + time;
            Vector3 gPos = g.transform.position;
            Vector3 diference = destination - gPos;
            while (Time.time < endTime)
            {
                float var = (endTime - Time.time) / time;
                g.transform.position = gPos + diference * (1 - var);
                yield return null;
            }
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        private void EndPhaseCommon()
        {
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);

            List<GameObject> runners = GameplayPlayerManager.Instance.Runners;

            foreach (GameObject g in runners)
            {
                RunnerCharacterController tempController = g.GetComponent<RunnerCharacterController>();
                tempController.SetLimitScreenRight(false);
                tempController.SetLimitScreenLeft(false);
                tempController.SetCheckOutScreen(false);
            }
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            EndPhaseCommon();
        }
    }
}
