using UnityEngine;
using System.Collections.Generic;

namespace Run4YourLife.DebuggingTools
{
    public class DebugSystemManager : MonoBehaviour
    {
        private bool debugging = false;

        private List<DebugFeature> generalDebugFeatures = new List<DebugFeature>();
        private List<DebugFeature> bossDebugFeatures = new List<DebugFeature>();
        private List<DebugFeature> cameraDebugFeatures = new List<DebugFeature>();
        private List<DebugFeature> otherDebugFeatures = new List<DebugFeature>();

        #region GUI definitions Window

        private const float WINDOW_OFFSET_X = 10.0f;
        private const float WINDOW_OFFSET_Y = 10.0f;
        private const float WINDOW_W = 300.0f;
        private const float WINDOW_H = 300.0f;

        private Rect windowRect = new Rect(Screen.width - WINDOW_W - WINDOW_OFFSET_X, WINDOW_OFFSET_Y, WINDOW_W, WINDOW_H);

        private Vector2 scrollAreaPosition = Vector2.zero;

        private bool drawGeneral = true;
        private bool drawBoss = true;
        private bool drawCamera = true;
        private bool drawOther = true;

        #endregion

        void Awake()
        {
            AddDebugFeatures();
        }

        void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                debugging = !debugging;
            }
        }

        private void AddDebugFeatures()
        {
            AddGeneralDebugFeatures();
            AddBossDebugFeatures();
            AddCameraDebugFeatures();
            AddOtherDebugFeatures();
        }

        private void AddGeneralDebugFeatures()
        {
            generalDebugFeatures.Add(gameObject.AddComponent<FPSCounter>());
            generalDebugFeatures.Add(gameObject.AddComponent<VertexAndTriangleCounter>());
            generalDebugFeatures.Add(Camera.main.gameObject.AddComponent<WireframeMode>());
        }

        private void AddBossDebugFeatures()
        {
            bossDebugFeatures.Add(gameObject.AddComponent<WalkerController>());
        }

        private void AddCameraDebugFeatures()
        {
            cameraDebugFeatures.Add(Camera.main.gameObject.AddComponent<DynamicCameraController>());
        }

        private void AddOtherDebugFeatures()
        {
            otherDebugFeatures.Add(gameObject.AddComponent<PhaseSwitcher>());
        }

        #region GUI

        void OnGUI()
        {
            if(debugging)
            {
                windowRect = GUI.Window(0, windowRect, OnGUIWindow, "Debugging Tools");
            }
        }

        private void OnGUIWindow(int windowID)
        {
            scrollAreaPosition = GUILayout.BeginScrollView(scrollAreaPosition);

            OnDrawGUI(ref drawGeneral, "General", generalDebugFeatures);
            OnDrawGUI(ref drawBoss, "Boss", bossDebugFeatures);
            OnDrawGUI(ref drawCamera, "Camera", cameraDebugFeatures);
            OnDrawGUI(ref drawOther, "Other", otherDebugFeatures);

            GUILayout.EndScrollView();

            GUI.DragWindow();
        }

        private void OnDrawGUI(ref bool draw, string name, List<DebugFeature> debuggingFeatures)
        {
            GUILayout.BeginVertical();

            if(GUILayout.Button(draw ? name : "< " + name + " >"))
            {
                draw = !draw;
            }

            if(draw)
            {
                foreach(DebugFeature debugFeature in debuggingFeatures)
                {
                    debugFeature.OnDrawGUI();
                }
            }

            GUILayout.EndVertical();
        }

        #endregion
    }
}