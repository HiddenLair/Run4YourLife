using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class EasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        #region Member variables

        [SerializeField]
        private GameObject m_phase1to2Bridge;

        [SerializeField]
        private GameObject m_phase2StartTrigger;

        [SerializeField]
        private CameraBossFollow m_cameraBossFollow;

        #endregion

        #region Initialization

        private void Awake()
        {
            RegisterPhase(GamePhase.EasyMoveHorizontal);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss"); //TODO make class with all tags and reference that
            Debug.Assert(boss != null);
            m_cameraBossFollow.boss = boss.transform;
            m_cameraBossFollow.enabled = true;

            m_phase1to2Bridge.SetActive(true);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            m_cameraBossFollow.enabled = false;
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            StartPhase();
        }

        public override void DebugEndPhase()
        {
            GameObject boss = FindObjectOfType<Boss>().gameObject;
            Debug.Assert(boss != null);
            Destroy(boss);

            PlayerCharacterController[] players = FindObjectsOfType<PlayerCharacterController>();
            foreach (PlayerCharacterController player in players)
            {
                Destroy(player.gameObject);
            }
            m_cameraBossFollow.enabled = false;
        }

        #endregion
    }
}