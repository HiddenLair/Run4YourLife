using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(BossStandControllerControlScheme))]
    public class BossStandController : PlayerStandController
    {
        #region Stands

        [SerializeField]
        private GameObject boss;

        #endregion

        protected override GameObject GetStandPrefabForPlayer(PlayerHandle playerHandle)
        {
            return boss;
        }

        protected override void UpdatePlayer()
        {
            base.UpdatePlayer();

            BossStandControllerControlScheme controlScheme = this.controlScheme as BossStandControllerControlScheme;

            if(!ready)
            {
                if(controlScheme.SetAsRunner.Started())
                {
                    PlayerStandsManager.Instance.SetAsRunner(this);
                }
            }
        }
    }
}