using UnityEngine;

namespace Run4YourLife.Debugging
{
    public abstract class DebugFeature : MonoBehaviour
    {
        private bool displaying = true;

        public void OnDrawGUI()
        {
            GUI.color = displaying ? Color.green : Color.red;

            if(GUILayout.Button(GetPanelName()))
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

        protected abstract string GetPanelName();

        protected abstract void OnCustomDrawGUI();
    }
}