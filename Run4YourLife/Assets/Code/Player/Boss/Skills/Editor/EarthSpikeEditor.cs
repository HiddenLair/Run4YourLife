using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Run4YourLife.Player;

namespace Run4YourLife.Player.CustomEditors
{
    [CustomEditor(typeof(EarthSpike))]
    [CanEditMultipleObjects]
    public class EarthSpikeEditor : SkillBaseEditor
    {
        SerializedProperty width;
        SerializedProperty delayHit;
        SerializedProperty timeToGrow;
        SerializedProperty timeToBreak;
        SerializedProperty adviseParticles;
        SerializedProperty earthPikeEffect;
        private void OnEnable()
        {
            base.Init();
            Init();
        }

        new public void Init()
        {
            width = serializedObject.FindProperty("width");
            delayHit = serializedObject.FindProperty("delayHit");
            timeToGrow = serializedObject.FindProperty("timeToGrow"); 
            timeToBreak = serializedObject.FindProperty("timeToBreak");
            adviseParticles = serializedObject.FindProperty("adviseParticles");
            earthPikeEffect = serializedObject.FindProperty("earthPikeEffect");
        }

        public override void OnGuiPhase1()
        {
            EditorGUILayout.PropertyField(width);
            EditorGUILayout.PropertyField(delayHit);
            EditorGUILayout.PropertyField(timeToGrow);
            EditorGUILayout.PropertyField(timeToBreak);
            EditorGUILayout.PropertyField(adviseParticles);
            EditorGUILayout.PropertyField(earthPikeEffect);
        }
        public override void OnGuiPhase2()
        {
            base.OnGuiPhase2();
        }

        public override void OnGuiPhase3()
        {
            base.OnGuiPhase3();
        }
    }
}