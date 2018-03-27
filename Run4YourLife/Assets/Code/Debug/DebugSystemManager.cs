using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class DebugSystemManager : MonoBehaviour
    {
        #region General

        private FPSCounter fpsCounter = null;

        private WireframeMode wireframeMode = null;

        private VertexAndTriangleCounter vertexAndTriangleCounter = null;

        #endregion

        #region Boss

        private WalkerController walkerController = null;

        #endregion

        private bool debugging = false;

        private bool drawWireframe = false;

        private string walkerSpeedText = string.Empty;
        private string walkerIncreaseSpeedText = string.Empty;

        void Start()
        {
            // ToggleDebugging(); // TEST
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
            fpsCounter = ToggleMode<FPSCounter>(fpsCounter, gameObject);
            vertexAndTriangleCounter = ToggleMode<VertexAndTriangleCounter>(vertexAndTriangleCounter, gameObject);
        }

        private void ToggleBossDebugging()
        {
            walkerController = ToggleMode<WalkerController>(walkerController, gameObject);
        }

        private void ToggleRunnerDebugging()
        {

        }

        private void ToggleOtherDebugging()
        {

        }

        private T ToggleMode<T>(MonoBehaviour mode, GameObject attachToObject) where T : Component
        {
            Debug.Assert(attachToObject != null); // TEST

            T ret = null;

            if(mode == null)
            {
                ret = attachToObject.AddComponent<T>();
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
                GUILayout.Label("Fps: " + fpsCounter.GetFPS().ToString("0") + ", Ms: " + fpsCounter.GetMs().ToString("0"));
                GUILayout.Label("Num. Vertices: " + vertexAndTriangleCounter.GetVertexCount());
                GUILayout.Label("Num. Triangles: " + vertexAndTriangleCounter.GetTriangleCount());

                bool newDrawWireframe = GUILayout.Toggle(drawWireframe, " Wireframe Mode");

                if(drawWireframe != newDrawWireframe)
                {
                    drawWireframe = newDrawWireframe;
                    wireframeMode = ToggleMode<WireframeMode>(wireframeMode, Camera.main.gameObject);
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
                if(walkerController.Exists())
                {
                    // Walker Speed

                    GUILayout.Label("Current Walker Speed: " + walkerController.GetSpeed().ToString("0.###"));

                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Walker Speed");
                    walkerSpeedText = GUILayout.TextField(walkerSpeedText);
                    if(GUILayout.Button("Apply"))
                    {
                        walkerController.SetSpeed(walkerSpeedText);
                    }

                    GUILayout.EndHorizontal();

                    // Walker Increase Speed

                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Walker Inc. Speed");
                    walkerIncreaseSpeedText = GUILayout.TextField(walkerIncreaseSpeedText);
                    if(GUILayout.Button("Apply"))
                    {
                        walkerController.SetIncreaseSpeed(walkerIncreaseSpeedText);
                    }

                    GUILayout.EndHorizontal();
                }
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