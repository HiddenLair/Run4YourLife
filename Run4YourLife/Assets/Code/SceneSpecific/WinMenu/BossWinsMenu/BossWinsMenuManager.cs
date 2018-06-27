using Run4YourLife.GameManagement.AudioManagement;
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
            if(m_sceneMusic != null)
            {
                AudioManager.Instance.PlayMusic(m_sceneMusic);
            }

            bossAnimator.Play(bossAnimation);
        }
    }
}