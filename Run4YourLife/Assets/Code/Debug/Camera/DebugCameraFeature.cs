using Cinemachine;

namespace Run4YourLife.DebuggingTools
{
    public abstract class DebugCameraFeature : DebugFeature
    {
        private CinemachineBrain cinemachineBrain = null;

        protected bool active = false;

        void Awake()
        {
            cinemachineBrain = GetComponent<CinemachineBrain>();
        }

        protected void Enable()
        {
            active = true;
            cinemachineBrain.enabled = false;

            foreach(DebugCameraFeature debugCameraFeature in GetComponents<DebugCameraFeature>())
            {
                if(debugCameraFeature != this)
                {
                    debugCameraFeature.active = false;
                }
            }
        }

        protected void Disable()
        {
            active = false;
            cinemachineBrain.enabled = true;
        }
    }
}