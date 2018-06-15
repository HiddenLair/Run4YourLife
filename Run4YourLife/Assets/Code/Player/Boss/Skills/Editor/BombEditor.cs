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
        SerializedProperty fireGameObject;
        SerializedProperty timeBetweenFire;
        SerializedProperty fireDuration;
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
            fireGameObject = serializedObject.FindProperty("fireGameObject");
            timeBetweenFire = serializedObject.FindProperty("timeBetweenFire");
            fireDuration = serializedObject.FindProperty("fireDuration");
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
            EditorGUILayout.PropertyField(fireGameObject);
            EditorGUILayout.PropertyField(timeBetweenFire);
            EditorGUILayout.PropertyField(fireDuration);
        }

        public override void OnGuiPhase3()
        {
            EditorGUILayout.PropertyField(timeBetweenJumps);
            EditorGUILayout.PropertyField(jumpHeight);
        }
    }
}