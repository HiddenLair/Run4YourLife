using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Run4YourLife.DebuggingTools
{
    public class DynamicCameraController : MonoBehaviour
    {
        private const float SPEED = 10.0f;
        private const float SHIFT_SPEED_MULTIPLIER = 5.0f;

        private new Camera camera = null;
        private CinemachineBrain cinemachineBrain = null;

        void Awake()
        {
            camera = GetComponent<Camera>();
            cinemachineBrain = GetComponent<CinemachineBrain>();

            cinemachineBrain.enabled = false;
        }

        void Update()
        {
            bool shiftPressed = UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
            float speed = SPEED * (shiftPressed ? SHIFT_SPEED_MULTIPLIER : 1.0f);

            Translate(speed);
            Zoom(speed);
        }

        void OnDestroy()
        {
            cinemachineBrain.enabled = true;
        }

        public void Focus()
        {
            // Can this be done in a better way? Ideas?

            StartCoroutine(EnableDisableCinemachine());
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

        private IEnumerator EnableDisableCinemachine()
        {
            // Can this be done in a better way? Ideas?

            yield return null;

            cinemachineBrain.enabled = true;

            yield return null;

            cinemachineBrain.enabled = false;
        }
    }
}