using System;
using UnityEngine;

using Run4YourLife.Input;
using Run4YourLife.Player;

namespace Run4YourLife.CharacterSelection
{
    [RequireComponent(typeof(RunnerStandControllerControlScheme))]
    public class RunnerStandController : PlayerStandController
    {
        [SerializeField]
        private ScaleTick scaleTickArrowLeft;

        [SerializeField]
        private ScaleTick scaleTickArrowRight;

        private RunnerPrefabManager runnerPrefabManager;

        protected override void OnAwake()
        {
            runnerPrefabManager = FindObjectOfType<RunnerPrefabManager>();
            Debug.Assert(runnerPrefabManager != null);
        }

        protected override GameObject GetStandPrefabForPlayer(PlayerDefinition playerDefinition)
        {
            return runnerPrefabManager.Get(RunnerPrefabType.CharacterSelection, playerDefinition.CharacterType);
        }

        protected override void UpdatePlayer()
        {
            base.UpdatePlayer();

            RunnerStandControllerControlScheme controlScheme = this.controlScheme as RunnerStandControllerControlScheme;

            if(!ready)
            {
                if(controlScheme.NextStand.Started())
                {
                    scaleTickArrowRight.Tick();
                    ChangePlayerCharacter(AdvanceType.Next);
                }
                else if(controlScheme.PreviousStand.Started())
                {
                    scaleTickArrowLeft.Tick();
                    ChangePlayerCharacter(AdvanceType.Previous);
                }
                else if(controlScheme.SetAsBoss.Started())
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