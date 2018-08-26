using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    public class BossWinsMenuManager : WinMenuManager
    {
        [SerializeField]
        private GameObject[] bossPrefabs;

        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private string bossAnimation = "Laugh";

        void Awake()
        {
            if(m_sceneMusic != null)
            {
                AudioManager.Instance.PlayMusic(m_sceneMusic);
            }

            if(m_characterSound != null)
            {
                AudioManager.Instance.PlaySFX(m_characterSound);
            }

            GameObject boss = Instantiate(GetBossPrefab(), spawnPoint, false);
            boss.GetComponent<Animator>().Play(bossAnimation);
        }

        private GameObject GetBossPrefab()
        {
            int bossIndex = 0;

            object currentBossId = GlobalDataContainer.Instance.Get("CurrentBossId");

            if(currentBossId != null)
            {
                bossIndex = (int)currentBossId;
            }

            return bossPrefabs[bossIndex];
        }
    }
}