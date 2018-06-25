using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public class Melee1 : Melee
    {
        [SerializeField]
        private TrembleConfig trembleValues;
        protected override void ExecuteMelee()
        {
            m_animator.SetTrigger(BossAnimation.Triggers.Melee);
            AudioManager.Instance.PlaySFX(m_meleeClip);
            StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator,BossAnimation.StateNames.Mele,0.6f,()=>TrembleManager.Instance.Tremble(trembleValues)));
        }
    }
}