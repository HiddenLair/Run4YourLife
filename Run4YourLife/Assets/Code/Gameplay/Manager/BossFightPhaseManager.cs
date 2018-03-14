using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class BossFightPhaseManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private CameraTargetCentered m_cameraTargetCentered;

        #endregion

        #region Member variables

        private PlayerSpawner m_playerSpawner;

        #endregion

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            RegisterPhase(GamePhase.BossFight);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            Debug.Log("Start Boss");

            GameObject boss = m_playerSpawner.InstantiateBossPlayer();
            m_cameraTargetCentered.m_target = boss.transform; // TODO: Temporal camera attachment
            m_cameraTargetCentered.enabled = true;
        }

        public override void EndPhase()
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            Destroy(boss);
            m_cameraTargetCentered.enabled = false;
        }

        #endregion

        #region Debug Execution

        public override void DebugEndPhase()
        {
            GameObject boss = FindObjectOfType<Boss2>().gameObject;
            Debug.Assert(boss != null);
            Destroy(boss);

            PlayerCharacterController[] players = FindObjectsOfType<PlayerCharacterController>();
            foreach (PlayerCharacterController player in players)
            {
                Destroy(player.gameObject);
            }
            m_cameraTargetCentered.enabled = false;
        }

        public override void DebugStartPhase()
        {
            m_playerSpawner.InstantiatePlayers();

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            m_cameraTargetCentered.m_target = boss.transform; // TODO: Temporal camera attachment
            m_cameraTargetCentered.enabled = true;
        }
        #endregion
    }
}
