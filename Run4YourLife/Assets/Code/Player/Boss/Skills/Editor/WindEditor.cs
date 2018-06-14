using UnityEditor;

namespace Run4YourLife.Player.CustomEditors
{
    [CustomEditor(typeof(Wind))]
    [CanEditMultipleObjects]
    public class WindEditor : SkillBaseEditor
    {
        SerializedProperty windMaxForceRelative;
        SerializedProperty windMinForceRelative;
        SerializedProperty screenMinWind;
        SerializedProperty windDuration;
        SerializedProperty windFillScreenTime;
        SerializedProperty windParticles;

        private void OnEnable()
        {
            base.Init();
            Init();
        }

        new public void Init()
        {
            windMaxForceRelative = serializedObject.FindProperty("windMaxForceRelative");
            windMinForceRelative = serializedObject.FindProperty("windMinForceRelative");
            screenMinWind = serializedObject.FindProperty("screenMinWind");
            windDuration = serializedObject.FindProperty("windDuration");
            windFillScreenTime = serializedObject.FindProperty("windFillScreenTime");
            windParticles = serializedObject.FindProperty("windParticles");
        }

        public override void OnGuiPhase1()
        {
            EditorGUILayout.PropertyField(windMaxForceRelative);
            EditorGUILayout.PropertyField(windMinForceRelative);
            EditorGUILayout.PropertyField(screenMinWind);
            EditorGUILayout.PropertyField(windDuration);
            EditorGUILayout.PropertyField(windFillScreenTime);
            EditorGUILayout.PropertyField(windParticles);
        }
        public override void OnGuiPhase2()
        {
        }

        public override void OnGuiPhase3()
        {
            base.OnGuiPhase3();
        }
    }
}