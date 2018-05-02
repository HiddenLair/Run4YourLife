using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Player
{
    public abstract class SkillBase : MonoBehaviour {

        [SerializeField]
        private float m_cooldown;

        public float Cooldown { get { return m_cooldown; } }
    }
}