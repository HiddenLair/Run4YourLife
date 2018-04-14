using System;
using UnityEngine;

using Run4YourLife.Input;
using Run4YourLife.Player;

namespace Run4YourLife.CharacterSelection
{
    [RequireComponent(typeof(RunnerStandControllerControlScheme))]
    public class RunnerStandController : PlayerStandController
    {
        #region Stands

        [SerializeField]
        private GameObject purpleRunner;

        [SerializeField]
        private GameObject greenRunner;

        [SerializeField]
        private GameObject orangeRunner;

        #endregion

        [SerializeField]
        private ScaleTick scaleTickArrowLeft;

        [SerializeField]
        private ScaleTick scaleTickArrowRight;

        protected override GameObject GetStandPrefabForPlayer(PlayerDefinition playerDefinition)
        {
            GameObject prefab = null;

            switch(playerDefinition.CharacterType)
            {
                case CharacterType.Purple:
                    prefab = purpleRunner;
                    break;
                case CharacterType.Green:
                    prefab = greenRunner;
                    break;
                case CharacterType.Orange:
                    prefab = orangeRunner;
                    break;
            }

            Debug.Assert(prefab != null);

            return prefab;
        }

        protected override void UpdatePlayer()
        {
            base.UpdatePlayer();

            RunnerStandControllerControlScheme controlScheme = this.controlScheme as RunnerStandControllerControlScheme;

            if(!ready)
            {
                if(controlScheme.nextStand.Started())
                {
                    scaleTickArrowRight.Tick();
                    ChangePlayerCharacter(AdvanceType.Next);
                }
                else if(controlScheme.previousStand.Started())
                {
                    scaleTickArrowLeft.Tick();
                    ChangePlayerCharacter(AdvanceType.Previous);
                }
                else if(controlScheme.setAsBoss.Started())
                {
                    playerStandsManager.SetAsBoss(this);
                }
            }
        }

        private enum AdvanceType
        {
            Next = 1,
            Previous = -1
        }

        private void ChangePlayerCharacter(AdvanceType advanceType)
        {
            PlayerDefinition playerDefinition = this.playerDefinition;
            ClearPlayerDefinition();
            playerDefinition.CharacterType = GetDeltaCharacterType(playerDefinition.CharacterType, (int)advanceType);
            SetPlayerDefinition(playerDefinition);
        }

        private CharacterType GetDeltaCharacterType(CharacterType characterType, int delta)
        {
            int nEnumElements = Enum.GetValues(typeof(CharacterType)).Length;
            int newCharacterTypeIndex = (nEnumElements + ((int)characterType) + delta) % nEnumElements;
            return (CharacterType)newCharacterTypeIndex;
        }
    }
}