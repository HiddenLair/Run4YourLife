using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(RunnerStandControllerControlScheme))]
    public class RunnerStandController : PlayerStandController
    {
        [SerializeField]
        private ScaleTick m_scaleTickArrowLeft;

        [SerializeField]
        private ScaleTick m_scaleTickArrowRight;

        [SerializeField]
        private string animationNotReady = "idle";

        [SerializeField]
        private string animationReady = "correr";

        private RunnerPrefabManager m_runnerPrefabManager;

        protected override void OnAwake()
        {
            m_runnerPrefabManager = FindObjectOfType<RunnerPrefabManager>();
            Debug.Assert(m_runnerPrefabManager != null);
        }

        protected override void OnReady()
        {
            activeStand.GetComponent<Animator>().Play(animationReady);
            m_scaleTickArrowLeft.gameObject.SetActive(false);
            m_scaleTickArrowRight.gameObject.SetActive(false);
        }

        protected override void OnNotReady()
        {
            activeStand.GetComponent<Animator>().Play(animationNotReady);
            m_scaleTickArrowLeft.gameObject.SetActive(true);
            m_scaleTickArrowRight.gameObject.SetActive(true);
        }

        protected override GameObject GetStandPrefabForPlayer(PlayerHandle playerHandle)
        {
            return m_runnerPrefabManager.Get(playerHandle.CharacterType);
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
            PlayerHandle playerHandle = this.playerHandle;
            ClearplayerHandle();
            playerHandle.CharacterType = PlayerManager.Instance.GetFirstAviableCharacterType(playerHandle.CharacterType, (int)advanceType);
            SetplayerHandle(playerHandle);
        }
    }
}