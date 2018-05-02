using UnityEngine;

namespace Run4YourLife.Debugging
{
    public class WalkerController : DebugFeature
    {
        private Walker walker = null;
        private GameObject boss = null;

        private string walkerSpeedText = string.Empty;
        private string walkerIncreaseSpeedText = string.Empty;

        protected override string GetPanelName()
        {
            return "Boss walker";
        }

        protected override void OnCustomDrawGUI()
        {
            if(walker != null)
            {
                // Walker Speed

                GUILayout.Label("Current Walker Speed: " + walker.speed.ToString("0.###"));

                GUILayout.BeginHorizontal();

                GUILayout.Label("Walker Speed");
                walkerSpeedText = GUILayout.TextField(walkerSpeedText);
                if(GUILayout.Button("Apply"))
                {
                    SetSpeed(walkerSpeedText);
                }

                GUILayout.EndHorizontal();

                // Walker Increase Speed

                GUILayout.BeginHorizontal();

                GUILayout.Label("Walker Inc. Speed");
                walkerIncreaseSpeedText = GUILayout.TextField(walkerIncreaseSpeedText);
                if(GUILayout.Button("Apply"))
                {
                    SetIncreaseSpeed(walkerIncreaseSpeedText);
                }

                GUILayout.EndHorizontal();
            }
        }

        void Update()
        {
            if(boss == null || !boss.activeInHierarchy)
            {
                FindBoss();

                if(boss != null)
                {
                    walker = boss.GetComponent<Walker>();
                }
            }
        }

        private void FindBoss()
        {
            GameObject[] bosses = GameObject.FindGameObjectsWithTag(Tags.Boss);

            foreach(GameObject currentBoss in bosses)
            {
                if(currentBoss.activeInHierarchy)
                {
                    boss = currentBoss;
                    break;
                }
            }
        }

        private void SetSpeed(string value)
        {
            float speed;

            if(float.TryParse(value, out speed))
            {
                walker.speed = speed;
            }
        }

        private void SetIncreaseSpeed(string value)
        {
            float increaseSpeed;

            if(float.TryParse(value, out increaseSpeed))
            {
                walker.increaseValue = increaseSpeed;
                walker.increaseSpeedOverTime = increaseSpeed != 0.0f;
            }
        }
    }
}