using UnityEngine;

namespace Run4YourLife.Debugging
{
    public abstract class DebugFeature : MonoBehaviour
    {
        private string cachedPanelNameStr = string.Empty;

        private bool displaying = true;

        void Awake()
        {
            cachedPanelNameStr = GetPanelName();

            OnAwake();
        }

        public void OnDrawGUI()
        {
            GUI.color = displaying ? Color.green : Color.red;

            if(GUILayout.Button(cachedPanelNameStr))
            {
                displaying = !displaying;
            }

            GUI.color = Color.white;

            if(displaying)
            {
                GUILayout.BeginVertical();

                OnCustomDrawGUI();

                GUILayout.EndVertical();
            }
        }

        protected virtual void OnAwake() { }

        protected abstract string GetPanelName();

        protected abstract void OnCustomDrawGUI();
    }
}