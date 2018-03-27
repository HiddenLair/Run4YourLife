using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.DebuggingTools
{
    public class PhaseSwitcher : MonoBehaviour
    {
        private GameManager gameManager = null;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void GoPhase1()
        {
            StartCoroutine(YieldHelper.SkipFrame(gameManager.DebugEndExecutingPhaseAndDebugStartPhase, GamePhase.EasyMoveHorizontal));
        }

        public void GoPhase2()
        {
            StartCoroutine(YieldHelper.SkipFrame(gameManager.DebugEndExecutingPhaseAndDebugStartPhase, GamePhase.BossFight));
        }

        public void GoPhase3()
        {
            StartCoroutine(YieldHelper.SkipFrame(gameManager.DebugEndExecutingPhaseAndDebugStartPhase, GamePhase.HardMoveHorizontal));
        }
    }
}