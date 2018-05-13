using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;
using UnityEngine.Serialization;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(BossStandControllerControlScheme))]
    public class BossStandController : PlayerStandController
    {
        [SerializeField]
        [FormerlySerializedAs("boss")]
        private GameObject m_bossStand;

        protected override GameObject GetStandPrefabForPlayer(PlayerHandle playerHandle)
        {
            return m_bossStand;
        }

        protected override void UpdatePlayer()
        {
            base.UpdatePlayer();

            BossStandControllerControlScheme controlScheme = this.m_playerStandControlScheme as BossStandControllerControlScheme;

            if(!IsReady && controlScheme.SetAsRunner.Started())
            {
                PlayerStandsManager.Instance.SetAsRunner(this);
            }
        }

        protected override void OnNotReady()
        {
        }

        protected override void OnReady()
        {
        }
    }
}