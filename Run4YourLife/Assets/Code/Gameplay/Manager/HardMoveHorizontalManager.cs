using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class HardMoveHorizontalManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private CheckPointManager m_checkPointManager;

        [SerializeField]
        private CameraBossFollow m_cameraBossFollow;

        #endregion

        #region Member Variables

        private PlayerSpawner m_playerSpawner;

        #endregion

        #region Regular Execution

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            RegisterPhase(GamePhase.HardMoveHorizontal);
        }

        #endregion

        public override void StartPhase()
        {
            m_checkPointManager.gameObject.SetActive(true);

            GameObject boss = m_playerSpawner.InstantiateBossPlayer();
            m_cameraBossFollow.boss = boss.transform;
            m_cameraBossFollow.enabled = true;
        }

        public override void EndPhase()
        {
            m_checkPointManager.gameObject.SetActive(false);

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            Destroy(boss);

            m_cameraBossFollow.boss = null;
            m_cameraBossFollow.enabled = false;
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_checkPointManager.gameObject.SetActive(true);

            GameObject[] players = m_playerSpawner.InstantiatePlayers();

            GameObject boss = players.Where(x => x.CompareTag("Boss")).First();
            m_cameraBossFollow.boss = boss.transform;
            m_cameraBossFollow.enabled = true;
        }

        public override void DebugEndPhase()
        {
            m_checkPointManager.gameObject.SetActive(false);

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            Destroy(boss);

            m_cameraBossFollow.boss = null;
            m_cameraBossFollow.enabled = false;
        }

        #endregion
    }
}

