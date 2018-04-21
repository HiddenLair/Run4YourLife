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
        private ScaleTick m_scaleTickArrowLeft;

        [SerializeField]
        private ScaleTick m_scaleTickArrowRight;

        private RunnerPrefabManager m_runnerPrefabManager;

        protected override void OnAwake()
        {
            m_runnerPrefabManager = FindObjectOfType<RunnerPrefabManager>();
            Debug.Assert(m_runnerPrefabManager != null);
        }

        protected override GameObject GetStandPrefabForPlayer(PlayerHandle playerDefinition)
        {
            return m_runnerPrefabManager.Get(playerDefinition.CharacterType);
        }

        protected override void UpdatePlayer()
        {
            base.UpdatePlayer();

            RunnerStandControllerControlScheme controlScheme = this.controlScheme as RunnerStandControllerControlScheme;

            if(!ready)
            {
                if(controlScheme.NextStand.Started())
                {
                    m_scaleTickArrowRight.Tick();
                    ChangePlayerCharacter(AdvanceType.Next);
                }
                else if(controlScheme.PreviousStand.Started())
                {
                    m_scaleTickArrowLeft.Tick();
                    ChangePlayerCharacter(AdvanceType.Previous);
                }
                else if(controlScheme.SetAsBoss.Started())
                {
                    PlayerStandsManager.Instance.SetAsBoss(this);
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
            PlayerHandle playerDefinition = this.playerDefinition;
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