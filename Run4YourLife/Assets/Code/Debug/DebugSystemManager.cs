using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class DebugSystemManager : MonoBehaviour
    {
        private GameObject mainCamera = null;

        private VertexAndTriangleCounter vertexAndTriangleCounter = null;

        private WireframeMode wireframeMode = null;

        private FPSCounter fpsCounter = null;

        private bool debugging = false;

        private bool drawWireframe = false;

        void Awake()
        {
            mainCamera = Camera.main.gameObject;
        }

        void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                ToggleDebugging();
            }
        }

        private void ToggleDebugging()
        {
            debugging = !debugging;

            if(drawGeneral)
            {
                ToggleGeneralDebugging();
            }

            if(drawBoss)
            {
                ToggleBossDebugging();
            }

            if(drawRunner)
            {
                ToggleRunnerDebugging();
            }

            if(drawOther)
            {
                ToggleOtherDebugging();
            }
        }

        private void ToggleGeneralDebugging()
        {
            fpsCounter = ToggleMode<FPSCounter>(fpsCounter);
            vertexAndTriangleCounter = ToggleMode<VertexAndTriangleCounter>(vertexAndTriangleCounter);
        }

        private void ToggleBossDebugging()
        {

        }

        private void ToggleRunnerDebugging()
        {

        }

        private void ToggleOtherDebugging()
        {

        }

        private T ToggleMode<T>(MonoBehaviour mode) where T : Component
        {
            T ret = null;

            if(mode == null)
            {
                ret = mainCamera.AddComponent<T>();
            }
            else
            {
                Destroy(mode);
            }

            return ret;
        }

        #region GUI definitions Window

        private const float WINDOW_OFFSET_X = 10.0f;
        private const float WINDOW_OFFSET_Y = 10.0f;
        private const float WINDOW_W = 300.0f;
        private const float WINDOW_H = 300.0f;

        private Rect windowRect = new Rect(Screen.width - WINDOW_W - WINDOW_OFFSET_X, WINDOW_OFFSET_Y, WINDOW_W, WINDOW_H);

        private Vector2 scrollAreaPosition = Vector2.zero;

        private bool drawGeneral = true;
        private bool drawBoss = true;
        private bool drawRunner = true;
        private bool drawOther = true;

        #endregion

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

            OnGUIGeneral();
            OnGUIBoss();
            OnGUIRunner();
            OnGUIOther();

            GUILayout.EndScrollView();

            GUI.DragWindow();
        }

        private void OnGUIGeneral()
        {
            GUILayout.BeginVertical();

            if(GUILayout.Button(drawGeneral ? "General" : "< General >"))
            {
                ToggleGeneralDebugging();
                drawGeneral = !drawGeneral;
            }

            if(drawGeneral)
            {
                GUILayout.Label("FPS: " + fpsCounter.GetFPS().ToString("0") + ", Ms: " + fpsCounter.GetMs().ToString("0"));
                GUILayout.Label("Num. Vertices: " + vertexAndTriangleCounter.GetVertexCount());
                GUILayout.Label("Num. Triangles: " + vertexAndTriangleCounter.GetTriangleCount());

                bool newDrawWireframe = GUILayout.Toggle(drawWireframe, " Wireframe Mode");

                if(drawWireframe != newDrawWireframe)
                {
                    drawWireframe = newDrawWireframe;
                    wireframeMode = ToggleMode<WireframeMode>(wireframeMode);
                }
            }

            GUILayout.EndVertical();
        }

        private void OnGUIBoss()
        {
            GUILayout.BeginVertical();

            if(GUILayout.Button(drawBoss ? "Boss" : "< Boss >"))
            {
                ToggleBossDebugging();
                drawBoss = !drawBoss;
            }

            if(drawBoss)
            {
                GUILayout.Label("To define...");
            }

            GUILayout.EndVertical();
        }

        private void OnGUIRunner()
        {
            GUILayout.BeginVertical();

            if(GUILayout.Button(drawRunner ? "Runner" : "< Runner >"))
            {
                ToggleRunnerDebugging();
                drawRunner = !drawRunner;
            }

            if(drawRunner)
            {
                GUILayout.Label("To define...");
            }

            GUILayout.EndVertical();
        }

        private void OnGUIOther()
        {
            GUILayout.BeginVertical();

            if(GUILayout.Button(drawOther ? "Other" : "< Other >"))
            {
                ToggleOtherDebugging();
                drawOther = !drawOther;
            }

            if(drawOther)
            {
                GUILayout.Label("To define...");
            }

            GUILayout.EndVertical();
        }

        #endregion
    }
}