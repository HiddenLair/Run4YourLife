﻿using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player
{
    public class Melee1 : Melee
    {
        protected override void ExecuteMelee()
        {
            m_animator.SetTrigger(BossAnimation.Triggers.Melee);
            AudioManager.Instance.PlaySFX(m_meleeClip);
        }
    }
}