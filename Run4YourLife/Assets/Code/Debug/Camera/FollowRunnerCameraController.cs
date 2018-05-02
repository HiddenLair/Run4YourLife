using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Debugging
{
    public class FollowRunnerCameraController : DebugCameraFeature
    {
        private const float SPEED = 5.0f;

        private GameObject followRunner = null;

        protected override string GetPanelName()
        {
            return "Follow runner camera";
        }

        protected override void OnCustomDrawGUI()
        {
            GUILayout.BeginHorizontal();

            foreach(GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                PlayerHandle runnerHandle = runner.GetComponent<PlayerInstance>().PlayerHandle;
                if(!runnerHandle.IsBoss)
                {
                    if(GUILayout.Button("Runner " + runnerHandle.ID))
                    {
                        Follow(runner);
                    }
                }
            }

            GUILayout.EndHorizontal();

            if(active)
            {
                if(GUILayout.Button("Stop"))
                {
                    Stop();
                }
            }
        }

        void Update()
        {
            if(active)
            {
                if(followRunner != null)
                {
                    Vector3 position = followRunner.transform.position;
                    position.z = transform.position.z;

                    transform.position = Vector3.Lerp(transform.position, position, SPEED * Time.deltaTime);
                }
                else
                {
                    Disable();
                }
            }
        }

        private void Follow(GameObject followGameObject)
        {
            Enable();
            followRunner = followGameObject;
        }

        private void Stop()
        {
            Disable();
            followRunner = null;
        }
    }
}