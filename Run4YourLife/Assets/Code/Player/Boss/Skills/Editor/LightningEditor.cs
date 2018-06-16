using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Run4YourLife.Player;

namespace Run4YourLife.Player.CustomEditors
{
    [CustomEditor(typeof(Lightning))]
    [CanEditMultipleObjects]
    public class LightningEditor : SkillBaseEditor
    {
        SerializedProperty width;
        SerializedProperty delayHit;
        SerializedProperty flashEffect;
        SerializedProperty lighningEffect;
        SerializedProperty delayBetweenLightnings;
        SerializedProperty delayBetweenLightningsProgresion;
        SerializedProperty trapGameObject;
        SerializedProperty newLightningsDelayHit;
        SerializedProperty newLightningsDelayHitProgresion;
        SerializedProperty newLightningsDistance;
        SerializedProperty newLightningsDistanceProgresion;
        SerializedProperty lightningGameObject;
        SerializedProperty maxNumberOfLightnings;

        private void OnEnable()
        {
            base.Init();
            Init();
        }

        new public void Init()
        {
            width = serializedObject.FindProperty("width");
            delayHit = serializedObject.FindProperty("delayHit");
            flashEffect = serializedObject.FindProperty("flashEffect");
            lighningEffect = serializedObject.FindProperty("lighningEffect");
            trapGameObject = serializedObject.FindProperty("trapGameObject");
            delayBetweenLightnings = serializedObject.FindProperty("delayBetweenLightnings"); ;
            delayBetweenLightningsProgresion = serializedObject.FindProperty("delayBetweenLightningsProgresion");
            newLightningsDelayHit = serializedObject.FindProperty("newLightningsDelayHit");
            newLightningsDelayHitProgresion = serializedObject.FindProperty("newLightningsDelayHitProgresion");
            newLightningsDistance = serializedObject.FindProperty("newLightningsDistance");
            newLightningsDistanceProgresion = serializedObject.FindProperty("newLightningsDistanceProgresion");
            lightningGameObject = serializedObject.FindProperty("lightningGameObject");
            maxNumberOfLightnings = serializedObject.FindProperty("maxNumberOfLightnings");
        }

        public override void OnGuiPhase1()
        {
            EditorGUILayout.PropertyField(width);
            EditorGUILayout.PropertyField(delayHit);
            EditorGUILayout.PropertyField(flashEffect);
            EditorGUILayout.PropertyField(lighningEffect);
        }

        public override void OnGuiPhase2()
        {
            EditorGUILayout.PropertyField(trapGameObject);
        }

        public override void OnGuiPhase3()
        {
            EditorGUILayout.PropertyField(delayBetweenLightnings);
            EditorGUILayout.PropertyField(delayBetweenLightningsProgresion);
            EditorGUILayout.PropertyField(newLightningsDelayHit);
            EditorGUILayout.PropertyField(newLightningsDelayHitProgresion);
            EditorGUILayout.PropertyField(newLightningsDistance);
            EditorGUILayout.PropertyField(newLightningsDistanceProgresion);
            EditorGUILayout.PropertyField(lightningGameObject);
            EditorGUILayout.PropertyField(maxNumberOfLightnings);
        }
    }
}
