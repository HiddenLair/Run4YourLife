using UnityEngine;
using System.Collections.Generic;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Debugging
{
    public static class DebugSystemManagerHelper
    {
        private static bool FORCE_DEBUGGING_TOOLS = false; // This should be false on release

        public static bool DebuggingToolsEnabled()
        {
            return FORCE_DEBUGGING_TOOLS || Debug.isDebugBuild;
        }
    }

    [RequireComponent(typeof(FPSCounter))]
    [RequireComponent(typeof(VertexAndTriangleCounter))]
    [RequireComponent(typeof(WalkerController))]
    [RequireComponent(typeof(PhaseSwitcher))]
    [RequireComponent(typeof(ParticleCounter))]
    public class DebugSystemManager : MonoBehaviour
    {
        private const string cachedWindowNameStr = "Debugging Tools";

        private List<DebugFeature> generalDebugFeatures = new List<DebugFeature>();
        private List<DebugFeature> bossDebugFeatures = new List<DebugFeature>();
        private List<DebugFeature> cameraDebugFeatures = new List<DebugFeature>();
        private List<DebugFeature> otherDebugFeatures = new List<DebugFeature>();

        private bool debugging = false;
        private int currentDebugFeaturesSetIndex = 0;
        private List<KeyValuePair<List<DebugFeature>, string>> debugFeatures = new List<KeyValuePair<List<DebugFeature>, string>>();

        #region GUI definitions Window

        private const float WINDOW_OFFSET_X = 10.0f;
        private const float WINDOW_OFFSET_Y = 10.0f;
        private const float WINDOW_W = 300.0f;
        private const float WINDOW_H = 300.0f;

        private Rect windowRect = new Rect(Screen.width - WINDOW_W - WINDOW_OFFSET_X, WINDOW_OFFSET_Y, WINDOW_W, WINDOW_H);

        #endregion

        void Awake()
        {
            if (DebugSystemManagerHelper.DebuggingToolsEnabled())
            {
                AddDebugFeatures();
            }
            else
            {
                Destroy(this);

                DebugFeature[] allDebugFeatures = FindObjectsOfType<DebugFeature>();
                for (int i = 0; i < allDebugFeatures.Length; ++i)
                {
                    Destroy(allDebugFeatures[i]);
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                debugging = !debugging;
                Cursor.visible = debugging;
            }
        }

        void OnDestroy()
        {
            Cursor.visible = false;
        }

        private void AddDebugFeatures()
        {
            AddGeneralDebugFeatures();
            AddBossDebugFeatures();
            AddCameraDebugFeatures();
            AddOtherDebugFeatures();

            debugFeatures.Add(new KeyValuePair<List<DebugFeature>, string>(generalDebugFeatures, "General"));
            debugFeatures.Add(new KeyValuePair<List<DebugFeature>, string>(bossDebugFeatures, "Boss"));
            debugFeatures.Add(new KeyValuePair<List<DebugFeature>, string>(cameraDebugFeatures, "Camera"));
            debugFeatures.Add(new KeyValuePair<List<DebugFeature>, string>(otherDebugFeatures, "Other"));
        }

        private void AddGeneralDebugFeatures()
        {
            generalDebugFeatures.Add(GetComponent<FPSCounter>());
            generalDebugFeatures.Add(GetComponent<VertexAndTriangleCounter>());
            generalDebugFeatures.Add(CameraManager.Instance.MainCamera.gameObject.AddComponent<WireframeMode>());
            generalDebugFeatures.Add(GetComponent<ParticleCounter>());
        }

        private void AddBossDebugFeatures()
        {
            bossDebugFeatures.Add(GetComponent<WalkerController>());
        }

        private void AddCameraDebugFeatures()
        {
            cameraDebugFeatures.Add(CameraManager.Instance.MainCamera.gameObject.AddComponent<DynamicCameraController>());
            cameraDebugFeatures.Add(CameraManager.Instance.MainCamera.gameObject.AddComponent<FollowRunnerCameraController>());
        }

        private void AddOtherDebugFeatures()
        {
            otherDebugFeatures.Add(GetComponent<PhaseSwitcher>());
            otherDebugFeatures.Add(GetComponent<PlayerInvencible>());
        }

        #region GUI

        void OnGUI()
        {
            if (debugging)
            {
                windowRect = GUI.Window(0, windowRect, OnGUIWindow, cachedWindowNameStr);
            }
        }

        private void OnGUIWindow(int windowID)
        {
            GUILayout.BeginHorizontal();

            for (int i = 0; i < debugFeatures.Count; ++i)
            {
                GUI.color = currentDebugFeaturesSetIndex == i ? Color.green : Color.red;

                if (GUILayout.Button(debugFeatures[i].Value))
                {
                    currentDebugFeaturesSetIndex = i;
                }
            }

            GUILayout.EndHorizontal();

            GUI.color = Color.white;

            GUILayout.BeginVertical();

            List<DebugFeature> currentDebugFeatures = debugFeatures[currentDebugFeaturesSetIndex].Key;

            foreach (DebugFeature debugFeature in currentDebugFeatures)
            {
                debugFeature.OnDrawGUI();
            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        #endregion
    }
}