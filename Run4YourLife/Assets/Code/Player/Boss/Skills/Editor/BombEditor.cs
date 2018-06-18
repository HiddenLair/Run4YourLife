using UnityEditor;

namespace Run4YourLife.Player.CustomEditors
{
    [CustomEditor(typeof(Bomb))]
    [CanEditMultipleObjects]
    public class BombEditor : SkillBaseEditor
    {
        SerializedProperty m_trapDetonationClip;
        SerializedProperty m_fadeInTime;
        SerializedProperty rayCheckerLenght;
        SerializedProperty gravity;
        SerializedProperty initialSpeed;
        SerializedProperty indicatorParticles;
        SerializedProperty m_explosionRatius;
        SerializedProperty activationParticles;
        SerializedProperty timeBetweenFire;
        SerializedProperty fireGrowDuration;
        SerializedProperty fireStableDuration;
        SerializedProperty fireScript;
        SerializedProperty timeBetweenJumps;
        SerializedProperty jumpHeight;

        private void OnEnable()
        {
            base.Init();
            Init();
        }

        new public void Init()
        {
            m_trapDetonationClip = serializedObject.FindProperty("m_trapDetonationClip");
            m_fadeInTime = serializedObject.FindProperty("m_fadeInTime");
            rayCheckerLenght = serializedObject.FindProperty("rayCheckerLenght");
            gravity = serializedObject.FindProperty("gravity");
            initialSpeed = serializedObject.FindProperty("initialSpeed");
            indicatorParticles = serializedObject.FindProperty("indicatorParticles");
            m_explosionRatius = serializedObject.FindProperty("m_explosionRatius");
            activationParticles = serializedObject.FindProperty("activationParticles");
            timeBetweenFire = serializedObject.FindProperty("timeBetweenFire");
            fireGrowDuration = serializedObject.FindProperty("fireGrowDuration");
            fireStableDuration = serializedObject.FindProperty("fireStableDuration");
            fireScript = serializedObject.FindProperty("fireScript");
            timeBetweenJumps = serializedObject.FindProperty("timeBetweenJumps");
            jumpHeight = serializedObject.FindProperty("jumpHeight");
        }

        public override void OnGuiPhase1()
        {
            EditorGUILayout.PropertyField(m_trapDetonationClip);
            EditorGUILayout.PropertyField(m_fadeInTime);
            EditorGUILayout.PropertyField(rayCheckerLenght);
            EditorGUILayout.PropertyField(gravity);
            EditorGUILayout.PropertyField(initialSpeed);
            EditorGUILayout.PropertyField(indicatorParticles);
            EditorGUILayout.PropertyField(m_explosionRatius);
            EditorGUILayout.PropertyField(activationParticles);
        }
        public override void OnGuiPhase2()
        {
            EditorGUILayout.PropertyField(timeBetweenFire);
            EditorGUILayout.PropertyField(fireGrowDuration);
            EditorGUILayout.PropertyField(fireStableDuration);
            EditorGUILayout.PropertyField(fireScript);
        }

        public override void OnGuiPhase3()
        {
            EditorGUILayout.PropertyField(timeBetweenJumps);
            EditorGUILayout.PropertyField(jumpHeight);
        }
    }
}