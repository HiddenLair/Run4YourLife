using UnityEngine;

namespace Run4YourLife.WinMenu
{
    public class BossWinsMenuManager : WinMenuManager
    {
        [SerializeField]
        private Animator bossAnimator;

        [SerializeField]
        private string bossAnimation = "Laugh";

        void Awake()
        {
            bossAnimator.Play(bossAnimation);
        }
    }
}