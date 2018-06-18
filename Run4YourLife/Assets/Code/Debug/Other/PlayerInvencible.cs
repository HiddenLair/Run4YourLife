using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Debugging
{
    public class PlayerInvencible : DebugFeature
    {
        private bool m_isInvincibleActive;

        private string m_invincibleActiveText = "Runners Invincible";

        protected override string GetPanelName()
        {
            return "Runner revive mode";
        }


        protected override void OnCustomDrawGUI()
        {
            m_isInvincibleActive = GUILayout.Toggle(m_isInvincibleActive, m_invincibleActiveText);
                        
            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                if(m_isInvincibleActive)
                {
                    Activate(runner);
                }
                else
                {
                    Deactivate(runner);
                }
            }
        }

        private void Activate(GameObject runner)
        {
            runner.GetComponent<RunnerController>().SetReviveMode(true);
            if (!runner.activeInHierarchy)
            {
                PlayerHandle playerHandle = runner.GetComponent<PlayerInstance>().PlayerHandle;
                GameplayPlayerManager.Instance.OnRunnerRevive(playerHandle, transform.position);
            }
        }

        private void Deactivate(GameObject runner)
        {
            runner.GetComponent<RunnerController>().SetReviveMode(false);
        }
    }
}
