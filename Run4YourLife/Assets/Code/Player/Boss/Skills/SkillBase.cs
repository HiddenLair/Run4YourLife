using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public abstract class SkillBase : MonoBehaviour {

        public struct SkillSpawnData {
            public Vector3 position;
            public Transform parent;
        }

        public enum Phase {PHASE1,PHASE2,PHASE3 };

        [SerializeField]
        protected Phase phase;

        [SerializeField]
        protected AudioClip m_skillTriggerClip;

        [SerializeField]
        private float m_cooldown;

        public float Cooldown { get { return m_cooldown; } }

        virtual public bool CanBePlacedAt(ref SkillSpawnData skillSpawnData)
        {
            return true;
        }

        virtual protected void Reset() { }

        virtual public void StartSkill() {
            Reset();
            StartSkillImplementation();
        }
        virtual protected void StartSkillImplementation() { }
    }
}