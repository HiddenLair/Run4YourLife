using UnityEditor;

namespace Run4YourLife.Player.CustomEditors
{
    [CustomEditor(typeof(Bomb))]
    [CanEditMultipleObjects]
    public class BombEditor : SkillBaseEditor
    {
        SerializedProperty m_trapfallClip;
        SerializedProperty m_trapDetonationClip;
        SerializedProperty m_fadeInTime;
        SerializedProperty rayCheckerLenght;
        SerializedProperty gravity;
        SerializedProperty initialSpeed;
        SerializedProperty indicatorParticles;
        SerializedProperty m_explosionRatius;
        SerializedProperty activationParticles;
        SerializedProperty trembleFall;
        SerializedProperty trembleExplosion;
        SerializedProperty timeBetweenFire;
        SerializedProperty fireGrowDuration;
        SerializedProperty fireStableDuration;
        SerializedProperty fireScript;
        SerializedProperty timeBetweenJumps;
        SerializedProperty jumpHeight;
        SerializedProperty bossSpawnCheckCollider;

        private void OnEnable()
        {
            base.Init();
            Init();
        }

        new public void Init()
        {
            m_trapfallClip = serializedObject.FindProperty("m_trapfallClip");
            m_trapDetonationClip = serializedObject.FindProperty("m_trapDetonationClip");
            m_fadeInTime = serializedObject.FindProperty("m_fadeInTime");
            rayCheckerLenght = serializedObject.FindProperty("rayCheckerLenght");
            gravity = serializedObject.FindProperty("gravity");
            initialSpeed = serializedObject.FindProperty("initialSpeed");
            indicatorParticles = serializedObject.FindProperty("indicatorParticles");
            m_explosionRatius = serializedObject.FindProperty("m_explosionRatius");
            activationParticles = serializedObject.FindProperty("activationParticles");
            trembleFall = serializedObject.FindProperty("trembleFall");
            trembleExplosion = serializedObject.FindProperty("trembleExplosion");
            timeBetweenFire = serializedObject.FindProperty("timeBetweenFire");
            fireGrowDuration = serializedObject.FindProperty("fireGrowDuration");
            fireStableDuration = serializedObject.FindProperty("fireStableDuration");
            fireScript = serializedObject.FindProperty("fireScript");
            timeBetweenJumps = serializedObject.FindProperty("timeBetweenJumps");
            jumpHeight = serializedObject.FindProperty("jumpHeight");
            bossSpawnCheckCollider = serializedObject.FindProperty("bossSpawnCheckCollider");
        }

        public override void OnGuiPhase1()
        {
            EditorGUILayout.PropertyField(m_trapfallClip);
            EditorGUILayout.PropertyField(m_trapDetonationClip);
            EditorGUILayout.PropertyField(m_fadeInTime);
            EditorGUILayout.PropertyField(rayCheckerLenght);
            EditorGUILayout.PropertyField(gravity);
            EditorGUILayout.PropertyField(initialSpeed);
            EditorGUILayout.PropertyField(indicatorParticles);
            EditorGUILayout.PropertyField(m_explosionRatius);
            EditorGUILayout.PropertyField(activationParticles);
            EditorGUILayout.PropertyField(trembleFall);
            EditorGUILayout.PropertyField(trembleExplosion);
            EditorGUILayout.PropertyField(bossSpawnCheckCollider);
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