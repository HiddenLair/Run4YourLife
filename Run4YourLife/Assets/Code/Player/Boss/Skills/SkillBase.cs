using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Run4YourLife.Player
{
    [CustomEditor(typeof(SkillBase),true)]
    [CanEditMultipleObjects]
    public class BaseSkillEditor : Editor
    {
        protected SerializedProperty phase;
        SerializedProperty m_skillTriggerClip;
        SerializedProperty m_cooldown;
        private void OnEnable()
        {
            Init();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(phase);
            EditorGUILayout.PropertyField(m_skillTriggerClip);
            EditorGUILayout.PropertyField(m_cooldown);

            serializedObject.ApplyModifiedProperties();
        }

        public void Init()
        {
            phase = serializedObject.FindProperty("phase");
            m_skillTriggerClip = serializedObject.FindProperty("m_skillTriggerClip");
            m_cooldown = serializedObject.FindProperty("m_cooldown");
        }
    }

    [RequireComponent(typeof(AudioSource))]
    public abstract class SkillBase : MonoBehaviour {

        public enum Phase {PHASE1,PHASE2,PHASE3 };

        [SerializeField]
        protected Phase phase;

        [SerializeField]
        protected AudioClip m_skillTriggerClip;

        protected AudioSource m_skillAudioSource;

        [SerializeField]
        private float m_cooldown;

        public float Cooldown { get { return m_cooldown; } }

        virtual public bool Check()
        {
            return true;
        }
    }
}