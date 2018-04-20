using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.DebuggingTools
{
    public class FollowRunnerCameraController : DebugCameraFeature
    {
        private const float SPEED = 5.0f;

        private GameObject followRunner = null;

        public override void OnDrawGUI()
        {
            GUILayout.BeginHorizontal();

            foreach(PlayerInstance playerInstance in FindObjectsOfType<PlayerInstance>())
            {
                PlayerHandle playerDefinition = playerInstance.PlayerHandle;

                if(!playerDefinition.IsBoss)
                {
                    if(GUILayout.Button("Runner " + playerDefinition.ID))
                    {
                        Follow(playerInstance.gameObject);
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