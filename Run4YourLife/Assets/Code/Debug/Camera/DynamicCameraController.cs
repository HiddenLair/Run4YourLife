using System;
using UnityEngine;
using Cinemachine;

using Run4YourLife.Utils;

namespace Run4YourLife.DebuggingTools
{
    public class DynamicCameraController : DebugFeature
    {
        private const float SPEED = 10.0f;
        private const float SHIFT_SPEED_MULTIPLIER = 5.0f;

        private new Camera camera = null;
        private CinemachineBrain cinemachineBrain = null;

        private bool dynamicCamera = false;

        public override void OnDrawGUI()
        {
            dynamicCamera = GUILayout.Toggle(dynamicCamera, " Dynamic Camera");

            if(dynamicCamera)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("Translate: Arrows\nZoom: W, S");
                if(GUILayout.Button("Focus"))
                {
                    Focus();
                }

                GUILayout.EndHorizontal();
            }
        }

        void Awake()
        {
            camera = GetComponent<Camera>();
            cinemachineBrain = GetComponent<CinemachineBrain>();
        }

        void Update()
        {
            cinemachineBrain.enabled = !dynamicCamera;

            if(dynamicCamera)
            {
                bool shiftPressed = UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
                float speed = SPEED * (shiftPressed ? SHIFT_SPEED_MULTIPLIER : 1.0f);

                Translate(speed);
                Zoom(speed);
            }
        }

        private void Translate(float speed)
        {
            Vector3 translate = new Vector3(Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.RightArrow)) - Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.LeftArrow)), Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.UpArrow)) - Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.DownArrow)), 0.0f);
            camera.transform.Translate(speed * Time.deltaTime * translate);
        }

        private void Zoom(float speed)
        {
            Vector3 zoom = new Vector3(0.0f, 0.0f, Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.W)) - Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.S)));
            camera.transform.Translate(speed * Time.deltaTime * zoom);
        }

        private void Focus()
        {
            // Can this be done in a better way? Ideas?

            StartCoroutine(YieldHelper.SkipFrame(() => cinemachineBrain.enabled = true));
        }
    }
}