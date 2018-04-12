using System;
using UnityEngine;

using Run4YourLife.Utils;

namespace Run4YourLife.DebuggingTools
{
    public class DynamicCameraController : DebugCameraFeature
    {
        private const float SPEED = 10.0f;
        private const float SHIFT_SPEED_MULTIPLIER = 5.0f;

        public override void OnDrawGUI()
        {
            bool tmpActive = active;
            active = GUILayout.Toggle(active, " Dynamic Camera");

            if(!tmpActive && active)
            {
                Enable();
            }
            else if(tmpActive && !active)
            {
                Disable();
            }

            if(active)
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

        void Update()
        {
            if(active)
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
            transform.Translate(speed * Time.deltaTime * translate);
        }

        private void Zoom(float speed)
        {
            Vector3 zoom = new Vector3(0.0f, 0.0f, Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.W)) - Convert.ToSingle(UnityEngine.Input.GetKey(KeyCode.S)));
            transform.Translate(speed * Time.deltaTime * zoom);
        }

        private void Focus()
        {
            // Can this be done in a better way? Ideas?

            StartCoroutine(YieldHelper.SkipFrame(() =>
            {
                Disable();
                StartCoroutine(YieldHelper.SkipFrame(() => Enable()));
            }));
        }
    }
}