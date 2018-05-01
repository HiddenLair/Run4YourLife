using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Debug
{
    public class PhaseSwitcher : DebugFeature
    {
        protected override string GetPanelName()
        {
            return "Phase switcher";
        }

        protected override void OnCustomDrawGUI()
        {
            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Go Phase 1"))
            {
                GoPhase(GamePhase.EasyMoveHorizontal);
            }
            else if(GUILayout.Button("Go Phase 2"))
            {
                GoPhase(GamePhase.BossFight);
            }
            else if(GUILayout.Button("Go Phase 3"))
            {
                GoPhase(GamePhase.HardMoveHorizontal);
            }

            GUILayout.EndHorizontal();
        }

        private void GoPhase(GamePhase gamePhase)
        {
            StartCoroutine(YieldHelper.SkipFrame(GameManager.Instance.DebugEndExecutingPhaseAndDebugStartPhase, gamePhase));
        }
    }
}