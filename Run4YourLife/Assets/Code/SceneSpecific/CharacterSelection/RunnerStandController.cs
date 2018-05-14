using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(RunnerStandControllerControlScheme))]
    public class RunnerStandController : PlayerStandController
    {
        [SerializeField]
        private PlayableDirector m_leftArrowUse;

        [SerializeField]
        private PlayableDirector m_rightArrowUse;

        [SerializeField]
        private string animationNotReady = "idle";

        [SerializeField]
        private string animationReady = "correr";

        private RunnerPrefabManager m_runnerPrefabManager;
        private bool wantsToBecomeBoss;

        protected override void Awake()
        {
            base.Awake();
            m_runnerPrefabManager = FindObjectOfType<RunnerPrefabManager>();
            Debug.Assert(m_runnerPrefabManager != null);
        }

        protected override void OnReady()
        {
            activeStand.GetComponent<Animator>().Play(animationReady);
            m_leftArrowUse.gameObject.SetActive(false);
            m_rightArrowUse.gameObject.SetActive(false);
        }

        protected override void OnNotReady()
        {
            activeStand.GetComponent<Animator>().Play(animationNotReady);
            m_leftArrowUse.gameObject.SetActive(true);
            m_rightArrowUse.gameObject.SetActive(true);
        }

        protected override GameObject GetStandPrefabForPlayer(PlayerHandle playerHandle)
        {
            return m_runnerPrefabManager.Get(playerHandle.CharacterType);
        }

        protected override void UpdatePlayer()
        {
            base.UpdatePlayer();

            RunnerStandControllerControlScheme controlScheme = this.m_playerStandControlScheme as RunnerStandControllerControlScheme;

            if(!IsReady)
            {
                if(controlScheme.NextStand.Started())
                {
                    m_rightArrowUse.Play();
                    ChangePlayerCharacter(AdvanceType.Next);
                }
                else if(controlScheme.PreviousStand.Started())
                {
                    m_leftArrowUse.Play();
                    ChangePlayerCharacter(AdvanceType.Previous);
                }
                else if(controlScheme.SetAsBoss.Started())
                {
                    wantsToBecomeBoss = true;
                }
            }
        }

        private void LateUpdate()
        {
            if(wantsToBecomeBoss)
            {
                PlayerStandsManager.Instance.SetAsBoss(this);
                wantsToBecomeBoss = false;
            }
        }

        private enum AdvanceType
        {
            Next = 1,
            Previous = -1
        }

        private void ChangePlayerCharacter(AdvanceType advanceType)
        {
            PlayerHandle playerHandle = this.PlayerHandle;
            playerHandle.CharacterType = PlayerManager.Instance.GetFirstAviableCharacterType(playerHandle.CharacterType, (int)advanceType);
            ExecuteEvents.Execute<IPlayerHandleEvent>(gameObject, null, (a,b) => a.OnPlayerHandleChanged(playerHandle));
        }
    }
}