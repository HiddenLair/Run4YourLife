using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player {
    [CustomEditor(typeof(Lightning))]
    [CanEditMultipleObjects]
    public class LightningEditor : BaseSkillEditor
    {
        SerializedProperty width;
        SerializedProperty delayHit;
        SerializedProperty flashEffect;
        SerializedProperty lighningEffect;
        SerializedProperty trapGameObject;
        SerializedProperty newLightningsDelay;
        SerializedProperty newLightningsDelayProgresion;
        SerializedProperty newLightningsDistance;
        SerializedProperty newLightningsDistanceProgresion;

        private void OnEnable()
        {
            base.Init();
            Init();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(width);
            EditorGUILayout.PropertyField(delayHit);
            EditorGUILayout.PropertyField(flashEffect);
            EditorGUILayout.PropertyField(lighningEffect);

            SkillBase.Phase actualPhase = (SkillBase.Phase)phase.intValue;
            if (actualPhase == SkillBase.Phase.PHASE2 || actualPhase == SkillBase.Phase.PHASE3)
            {
                EditorGUILayout.PropertyField(trapGameObject);
            }
            if (actualPhase == SkillBase.Phase.PHASE3)
            {
                EditorGUILayout.PropertyField(newLightningsDelay);
                EditorGUILayout.PropertyField(newLightningsDelayProgresion);
                EditorGUILayout.PropertyField(newLightningsDistance);
                EditorGUILayout.PropertyField(newLightningsDistanceProgresion);
            }
            serializedObject.ApplyModifiedProperties();
        }

        new public void Init()
        {
             width = serializedObject.FindProperty("width");
             delayHit = serializedObject.FindProperty("delayHit");
             flashEffect = serializedObject.FindProperty("flashEffect");
             lighningEffect = serializedObject.FindProperty("lighningEffect");
             trapGameObject = serializedObject.FindProperty("trapGameObject");
             newLightningsDelay = serializedObject.FindProperty("newLightningsDelay");
             newLightningsDelayProgresion = serializedObject.FindProperty("newLightningsDelayProgresion");
             newLightningsDistance = serializedObject.FindProperty("newLightningsDistance");
             newLightningsDistanceProgresion = serializedObject.FindProperty("newLightningsDistanceProgresion");
        }
    }

    public class Lightning : SkillBase
    {
        #region Inspector

        [SerializeField]
        private float width;

        [SerializeField]
        private float delayHit;

        [SerializeField]
        private GameObject flashEffect;

        [SerializeField]
        private GameObject lighningEffect;

        [SerializeField]
        private GameObject trapGameObject;
        [SerializeField]
        private float newLightningsDelay;
        [SerializeField]
        private float newLightningsDelayProgresion;
        [SerializeField]
        private float newLightningsDistance;
        [SerializeField]
        private float newLightningsDistanceProgresion;

        #endregion

        #region Private Variables

        private WaitForSeconds lightningDelay;

        #endregion

        private void Awake()
        {
            lightningDelay = new WaitForSeconds(delayHit);
        }

        public override void  StartSkill()
        {
            Vector3 position = transform.position;
            position.y = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0, 0, Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.z - transform.position.z))).y;
            transform.position = position;
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            Transform flashBody = flashEffect.transform;
            Vector3 newSize = Vector3.one;
            newSize.x = newSize.z = width;
            float topScreen = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0, CameraManager.Instance.MainCamera.pixelHeight, Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.z - flashBody.position.z))).y;
            newSize.y = (topScreen - transform.position.y) / 2;
            flashBody.localScale = newSize;
            flashBody.localPosition = new Vector3(0, newSize.y);
            flashEffect.SetActive(true);
            yield return lightningDelay;
            flashEffect.SetActive(false);
            LightningHit();
        }

        private void LightningHit()
        {
            AudioManager.Instance.PlaySFX(m_skillTriggerClip);
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 pos = Vector3.zero;
            pos.y = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight, Mathf.Abs(mainCamera.transform.position.z - pos.z))).y;
            lighningEffect.transform.localPosition = pos;


            RaycastHit[] hits;
            hits = Physics.SphereCastAll(lighningEffect.transform.position, width, Vector3.down, pos.y - transform.position.y, Layers.Runner);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == Tags.Runner)
                {
                    ExecuteEvents.Execute<ICharacterEvents>(hit.collider.gameObject, null, (x, y) => x.Kill());
                }
            }
            lighningEffect.SetActive(true);
            ParticleSystem[] lightning = lighningEffect.GetComponentsInChildren<ParticleSystem>();
            StartCoroutine(WaitForParticleSystems(lightning,lighningEffect));
        }

        IEnumerator WaitForParticleSystems(ParticleSystem[] particles,GameObject particleSystem)
        {
            bool finish = false;
            while(!finish){
                finish = true;
                for (int i = 0; i < particles.Length; ++i)
                {
                    if (particles[i].IsAlive(false))
                    {
                        finish = false;
                        break;
                    }
                }
                yield return null;
            }
            particleSystem.SetActive(false);
            gameObject.SetActive(false);
        }

        public void SetDelayHit(float value)
        {
            delayHit = value;
        }

        public float GetDelayHit()
        {
            return delayHit;
        }
    }
}
