using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.DebuggingTools
{
    public class PhaseSwitcher : DebugFeature
    {
        private GameManager gameManager = null;

        public override void OnDrawGUI()
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

        void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void GoPhase(GamePhase gamePhase)
        {
            StartCoroutine(YieldHelper.SkipFrame(gameManager.DebugEndExecutingPhaseAndDebugStartPhase, gamePhase));
        }
    }
}