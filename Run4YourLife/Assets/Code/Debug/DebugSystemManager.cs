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

        private const float SCROLL_AREA_OFFSET_X = 0.0f;
        private const float SCROLL_AREA_OFFSET_Y = 20.0f;
        private const float SCROLL_AREA_W = WINDOW_W + 20.0f;
        private const float SCROLL_AREA_H = WINDOW_H - SCROLL_AREA_OFFSET_Y - 5.0f;

        private Rect scrollAreaRect = new Rect(SCROLL_AREA_OFFSET_X, SCROLL_AREA_OFFSET_Y, SCROLL_AREA_W, SCROLL_AREA_H);

        private Vector2 scrollAreaPosition = Vector2.zero;

        private bool drawGeneral = true;
        private bool drawBoss = true;
        private bool drawRunner = true;
        private bool drawOther = true;

        #endregion

        #region GUI definitions General

        private const float GROUP_GENERAL_OFFSET_X = 10.0f;
        private const float GROUP_GENERAL_OFFSET_Y = 0.0f;
        private const float GROUP_GENERAL_W = WINDOW_W - 2.0f * GROUP_GENERAL_OFFSET_X;
        private const float GROUP_GENERAL_H = 125.0f;

        private Rect groupGeneralRect = new Rect(GROUP_GENERAL_OFFSET_X, GROUP_GENERAL_OFFSET_Y, GROUP_GENERAL_W, GROUP_GENERAL_H);

        private const float GROUP_GENERAL_BUTTON_OFFSET_X = 0.0f;
        private const float GROUP_GENERAL_BUTTON_OFFSET_Y = 0.0f;
        private const float GROUP_GENERAL_BUTTON_W = GROUP_GENERAL_W - 2.0f * GROUP_GENERAL_BUTTON_OFFSET_X;
        private const float GROUP_GENERAL_BUTTON_H = 20.0f;

        private Rect groupGeneralButtonRect = new Rect(GROUP_GENERAL_BUTTON_OFFSET_X, GROUP_GENERAL_BUTTON_OFFSET_Y, GROUP_GENERAL_BUTTON_W, GROUP_GENERAL_BUTTON_H);

        private const float GROUP_GENERAL_BOX_OFFSET_X = 0.0f;
        private const float GROUP_GENERAL_BOX_OFFSET_Y = 25.0f;
        private const float GROUP_GENERAL_BOX_W = GROUP_GENERAL_W - 2.0f * GROUP_GENERAL_BOX_OFFSET_X;
        private const float GROUP_GENERAL_BOX_H = GROUP_GENERAL_H - GROUP_GENERAL_BOX_OFFSET_Y;

        private Rect groupGeneralBoxRect = new Rect(GROUP_GENERAL_BOX_OFFSET_X, GROUP_GENERAL_BOX_OFFSET_Y, GROUP_GENERAL_BOX_W, GROUP_GENERAL_BOX_H);

        private const float GROUP_GENERAL_CONTENT_OFFSET_X = 10.0f;
        private const float GROUP_GENERAL_CONTENT_W = GROUP_GENERAL_BOX_W;
        private const float GROUP_GENERAL_CONTENT_H = 20.0f;

        private const float GROUP_GENERAL_FPS_OFFSET_Y = 35.0f;
        private const float GROUP_GENERAL_VERTICES_OFFSET_Y = GROUP_GENERAL_FPS_OFFSET_Y + GROUP_GENERAL_CONTENT_H;
        private const float GROUP_GENERAL_TRIANGLES_OFFSET_Y = GROUP_GENERAL_VERTICES_OFFSET_Y + GROUP_GENERAL_CONTENT_H;
        private const float GROUP_GENERAL_WIREFRAME_OFFSET_Y = GROUP_GENERAL_TRIANGLES_OFFSET_Y + GROUP_GENERAL_CONTENT_H;

        private Rect groupGeneralFPSRect = new Rect(GROUP_GENERAL_CONTENT_OFFSET_X, GROUP_GENERAL_FPS_OFFSET_Y, GROUP_GENERAL_CONTENT_W, GROUP_GENERAL_CONTENT_H);
        private Rect groupGeneralVerticesRect = new Rect(GROUP_GENERAL_CONTENT_OFFSET_X, GROUP_GENERAL_VERTICES_OFFSET_Y, GROUP_GENERAL_CONTENT_W, GROUP_GENERAL_CONTENT_H);
        private Rect groupGeneralTrianglesRect = new Rect(GROUP_GENERAL_CONTENT_OFFSET_X, GROUP_GENERAL_TRIANGLES_OFFSET_Y, GROUP_GENERAL_CONTENT_W, GROUP_GENERAL_CONTENT_H);
        private Rect groupGeneralWireframeRect = new Rect(GROUP_GENERAL_CONTENT_OFFSET_X, GROUP_GENERAL_WIREFRAME_OFFSET_Y, GROUP_GENERAL_CONTENT_W, GROUP_GENERAL_CONTENT_H);

        private bool drawWireframe = false;

        #endregion

        #region GUI definitions Boss

        private const float GROUP_BOSS_OFFSET_X = 10.0f;
        private const float GROUP_BOSS_OFFSET_Y = 5.0f;
        private const float GROUP_BOSS_W = WINDOW_W - 2.0f * GROUP_BOSS_OFFSET_X;
        private const float GROUP_BOSS_H = 125.0f;

        private Rect groupBossRect = new Rect(GROUP_BOSS_OFFSET_X, GROUP_BOSS_OFFSET_Y, GROUP_BOSS_W, GROUP_BOSS_H);

        private const float GROUP_BOSS_BUTTON_OFFSET_X = 0.0f;
        private const float GROUP_BOSS_BUTTON_OFFSET_Y = 0.0f;
        private const float GROUP_BOSS_BUTTON_W = GROUP_BOSS_W - 2.0f * GROUP_BOSS_BUTTON_OFFSET_X;
        private const float GROUP_BOSS_BUTTON_H = 20.0f;

        private Rect groupBossButtonRect = new Rect(GROUP_BOSS_BUTTON_OFFSET_X, GROUP_BOSS_BUTTON_OFFSET_Y, GROUP_BOSS_BUTTON_W, GROUP_BOSS_BUTTON_H);

        private const float GROUP_BOSS_BOX_OFFSET_X = 0.0f;
        private const float GROUP_BOSS_BOX_OFFSET_Y = 25.0f;
        private const float GROUP_BOSS_BOX_W = GROUP_BOSS_W - 2.0f * GROUP_BOSS_BOX_OFFSET_X;
        private const float GROUP_BOSS_BOX_H = GROUP_BOSS_H - GROUP_BOSS_BOX_OFFSET_Y;

        private Rect groupBossBoxRect = new Rect(GROUP_BOSS_BOX_OFFSET_X, GROUP_BOSS_BOX_OFFSET_Y, GROUP_BOSS_BOX_W, GROUP_BOSS_BOX_H);

        private const float GROUP_BOSS_CONTENT_OFFSET_X = 10.0f;
        private const float GROUP_BOSS_CONTENT_W = GROUP_BOSS_BOX_W;
        private const float GROUP_BOSS_CONTENT_H = 20.0f;

        #endregion

        #region GUI definitions Runner

        private const float GROUP_RUNNER_OFFSET_X = 10.0f;
        private const float GROUP_RUNNER_OFFSET_Y = 10.0f;
        private const float GROUP_RUNNER_W = WINDOW_W - 2.0f * GROUP_RUNNER_OFFSET_X;
        private const float GROUP_RUNNER_H = 125.0f;

        private Rect groupRunnerRect = new Rect(GROUP_RUNNER_OFFSET_X, GROUP_RUNNER_OFFSET_Y, GROUP_RUNNER_W, GROUP_RUNNER_H);

        private const float GROUP_RUNNER_BUTTON_OFFSET_X = 0.0f;
        private const float GROUP_RUNNER_BUTTON_OFFSET_Y = 0.0f;
        private const float GROUP_RUNNER_BUTTON_W = GROUP_RUNNER_W - 2.0f * GROUP_RUNNER_BUTTON_OFFSET_X;
        private const float GROUP_RUNNER_BUTTON_H = 20.0f;

        private Rect groupRunnerButtonRect = new Rect(GROUP_RUNNER_BUTTON_OFFSET_X, GROUP_RUNNER_BUTTON_OFFSET_Y, GROUP_RUNNER_BUTTON_W, GROUP_RUNNER_BUTTON_H);

        private const float GROUP_RUNNER_BOX_OFFSET_X = 0.0f;
        private const float GROUP_RUNNER_BOX_OFFSET_Y = 25.0f;
        private const float GROUP_RUNNER_BOX_W = GROUP_RUNNER_W - 2.0f * GROUP_RUNNER_BOX_OFFSET_X;
        private const float GROUP_RUNNER_BOX_H = GROUP_RUNNER_H - GROUP_RUNNER_BOX_OFFSET_Y;

        private Rect groupRunnerBoxRect = new Rect(GROUP_RUNNER_BOX_OFFSET_X, GROUP_RUNNER_BOX_OFFSET_Y, GROUP_RUNNER_BOX_W, GROUP_RUNNER_BOX_H);

        private const float GROUP_RUNNER_CONTENT_OFFSET_X = 10.0f;
        private const float GROUP_RUNNER_CONTENT_W = GROUP_RUNNER_BOX_W;
        private const float GROUP_RUNNER_CONTENT_H = 20.0f;

        #endregion

        #region GUI definitions Other

        private const float GROUP_OTHER_OFFSET_X = 10.0f;
        private const float GROUP_OTHER_OFFSET_Y = 15.0f;
        private const float GROUP_OTHER_W = WINDOW_W - 2.0f * GROUP_OTHER_OFFSET_X;
        private const float GROUP_OTHER_H = 125.0f;

        private Rect groupOtherRect = new Rect(GROUP_OTHER_OFFSET_X, GROUP_OTHER_OFFSET_Y, GROUP_OTHER_W, GROUP_OTHER_H);

        private const float GROUP_OTHER_BUTTON_OFFSET_X = 0.0f;
        private const float GROUP_OTHER_BUTTON_OFFSET_Y = 0.0f;
        private const float GROUP_OTHER_BUTTON_W = GROUP_OTHER_W - 2.0f * GROUP_OTHER_BUTTON_OFFSET_X;
        private const float GROUP_OTHER_BUTTON_H = 20.0f;

        private Rect groupOtherButtonRect = new Rect(GROUP_OTHER_BUTTON_OFFSET_X, GROUP_OTHER_BUTTON_OFFSET_Y, GROUP_OTHER_BUTTON_W, GROUP_OTHER_BUTTON_H);

        private const float GROUP_OTHER_BOX_OFFSET_X = 0.0f;
        private const float GROUP_OTHER_BOX_OFFSET_Y = 25.0f;
        private const float GROUP_OTHER_BOX_W = GROUP_OTHER_W - 2.0f * GROUP_OTHER_BOX_OFFSET_X;
        private const float GROUP_OTHER_BOX_H = GROUP_OTHER_H - GROUP_OTHER_BOX_OFFSET_Y;

        private Rect groupOtherBoxRect = new Rect(GROUP_OTHER_BOX_OFFSET_X, GROUP_OTHER_BOX_OFFSET_Y, GROUP_OTHER_BOX_W, GROUP_OTHER_BOX_H);

        private const float GROUP_OTHER_CONTENT_OFFSET_X = 10.0f;
        private const float GROUP_OTHER_CONTENT_W = GROUP_OTHER_BOX_W;
        private const float GROUP_OTHER_CONTENT_H = 20.0f;

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
            float[] offsetsY = ComputeOffsetsY();

            Rect fullScrollAreaRect = new Rect(0.0f, 0.0f, WINDOW_W, offsetsY[3] + SCROLL_AREA_OFFSET_Y + (drawOther ? GROUP_OTHER_H : GROUP_OTHER_BUTTON_H));

            scrollAreaPosition = GUI.BeginScrollView(scrollAreaRect, scrollAreaPosition, fullScrollAreaRect);

            OnGUIGeneral(offsetsY[0]);
            OnGUIBoss(offsetsY[1]);
            OnGUIRunner(offsetsY[2]);
            OnGUIOther(offsetsY[3]);

            GUI.EndScrollView();

            GUI.DragWindow();
        }

        private float[] ComputeOffsetsY()
        {
            float[] offsetsY = new float[4];

            offsetsY[0] = 0.0f;
            offsetsY[1] = offsetsY[0] + (drawGeneral ? GROUP_GENERAL_H : GROUP_GENERAL_BUTTON_H);
            offsetsY[2] = offsetsY[1] + (drawBoss ? GROUP_BOSS_H : GROUP_BOSS_BUTTON_H);
            offsetsY[3] = offsetsY[2] + (drawRunner ? GROUP_RUNNER_H : GROUP_RUNNER_BUTTON_H);

            return offsetsY;
        }

        private void OnGUIGeneral(float offsetY)
        {
            Rect groupRect = groupGeneralRect;
            groupRect.y += offsetY;

            GUI.BeginGroup(groupRect, string.Empty);

            if(GUI.Button(groupGeneralButtonRect, drawGeneral ? "General" : "< General >"))
            {
                ToggleGeneralDebugging();
                drawGeneral = !drawGeneral;
            }

            if(drawGeneral)
            {
                GUI.Box(groupGeneralBoxRect, string.Empty);

                GUI.Label(groupGeneralFPSRect, "FPS: " + fpsCounter.GetFPS().ToString("0") + ", Ms: " + fpsCounter.GetMs().ToString("0"));
                GUI.Label(groupGeneralVerticesRect, "Num. Vertices: " + vertexAndTriangleCounter.GetVertexCount());
                GUI.Label(groupGeneralTrianglesRect, "Num. Triangles: " + vertexAndTriangleCounter.GetTriangleCount());

                bool newDrawWireframe = GUI.Toggle(groupGeneralWireframeRect, drawWireframe, " Wireframe Mode");

                if(drawWireframe != newDrawWireframe)
                {
                    drawWireframe = newDrawWireframe;
                    wireframeMode = ToggleMode<WireframeMode>(wireframeMode);
                }
            }

            GUI.EndGroup();
        }

        private void OnGUIBoss(float offsetY)
        {
            Rect groupRect = groupBossRect;
            groupRect.y += offsetY;

            GUI.BeginGroup(groupRect, string.Empty);

            if(GUI.Button(groupBossButtonRect, drawBoss ? "Boss" : "< Boss >"))
            {
                ToggleBossDebugging();
                drawBoss = !drawBoss;
            }

            if(drawBoss)
            {
                GUI.Box(groupBossBoxRect, string.Empty);
            }

            GUI.EndGroup();
        }

        private void OnGUIRunner(float offsetY)
        {
            Rect groupRect = groupRunnerRect;
            groupRect.y += offsetY;

            GUI.BeginGroup(groupRect, string.Empty);

            if(GUI.Button(groupRunnerButtonRect, drawRunner ? "Runner" : "< Runner >"))
            {
                ToggleRunnerDebugging();
                drawRunner = !drawRunner;
            }

            if(drawRunner)
            {
                GUI.Box(groupRunnerBoxRect, string.Empty);
            }

            GUI.EndGroup();
        }

        private void OnGUIOther(float offsetY)
        {
            Rect groupRect = groupOtherRect;
            groupRect.y += offsetY;

            GUI.BeginGroup(groupRect, string.Empty);

            if(GUI.Button(groupOtherButtonRect, drawOther ? "Other" : "< Other >"))
            {
                ToggleOtherDebugging();
                drawOther = !drawOther;
            }

            if(drawOther)
            {
                GUI.Box(groupOtherBoxRect, string.Empty);
            }

            GUI.EndGroup();
        }

        #endregion
    }
}