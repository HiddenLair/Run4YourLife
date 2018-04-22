using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

namespace Run4YourLife.DebuggingTools
{
    public class RunnerGodMode : DebugFeature
    {
        [SerializeField]
        private GameObject runnerGodModePrefab;

        public override void OnDrawGUI()
        {
            GUILayout.BeginHorizontal();

            List<GameObject> runners = GameplayPlayerManager.Instance.RunnersAlive;

            for(int i = 0; i < runners.Count; ++i)
            {
                GameObject runner = runners[i];

                if(runner.activeInHierarchy)
                {
                    PlayerHandle runnerHandle = runner.GetComponent<PlayerInstance>().PlayerHandle;

                    if(GUILayout.Button("Runner " + runnerHandle.ID))
                    {
                        Activate(runner);
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        private void Activate(GameObject runner)
        {
            runner.SetActive(false);

            GameObject runnerGodModeInstance = Instantiate(runnerGodModePrefab);
            runnerGodModeInstance.GetComponent<RunnerGodModeController>().Begin(runner);
        }
    }
}