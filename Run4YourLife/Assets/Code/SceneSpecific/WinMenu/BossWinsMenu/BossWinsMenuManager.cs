using UnityEngine;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    public class BossWinsMenuManager : WinMenuManager
    {
        [SerializeField]
        private Animator bossAnimator;

        [SerializeField]
        private string bossAnimation = "Laugh";

        void Awake()
        {
            GlobalDataContainer.Instance.Data.Remove(GlobalDataContainerKeys.Score);
            bossAnimator.Play(bossAnimation);
        }
    }
}