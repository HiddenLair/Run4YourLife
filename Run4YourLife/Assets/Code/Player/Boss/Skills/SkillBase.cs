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

        virtual public bool CheckAndRepositionSkillSpawn(ref SkillSpawnData skillSpawnData)
        {
            return true;
        }

        virtual protected void ResetState() { }

        public void StartSkill() {
            ResetState();
            OnSkillStart();
        }
        virtual protected void OnSkillStart() { }
    }
}