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

        private bool wantsToBecomeRunner;

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
                wantsToBecomeRunner = true;
            }
        }

        private void LateUpdate()
        {
            if(wantsToBecomeRunner)
            {
                PlayerStandsManager.Instance.SetAsRunner(this);
                wantsToBecomeRunner = false;
            }
        }

        protected override void OnNotReady()
        {
            activeStand.GetComponent<BossEyesEnabler>().Enable(false);
        }

        protected override void OnReady()
        {
            activeStand.GetComponent<BossEyesEnabler>().Enable(true);
        }
    }
}