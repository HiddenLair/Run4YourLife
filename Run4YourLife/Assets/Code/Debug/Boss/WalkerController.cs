using System;
using System.Collections;

using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;

namespace Run4YourLife.Debugging
{
    public class WalkerController : DebugFeature
    {
        [SerializeField]
        private float m_manualSpeed;

        private string m_speedText;
        private string m_accelerationText;

        private bool isWalkerManual;


        protected override string GetPanelName()
        {
            return "Boss walker";
        }

        protected override void OnCustomDrawGUI()
        {
            if(GameplayPlayerManager.Instance.Boss != null)
            {
                BossPathWalker bossPathWalker = GameplayPlayerManager.Instance.Boss.GetComponent<BossPathWalker>();
                if(bossPathWalker != null)
                {
                    Speed(bossPathWalker);
                    Acceleration(bossPathWalker);
                    WalkerManual(bossPathWalker);
                }
            }
        }

        private void WalkerManual(BossPathWalker bossPathWalker)
        {
        }

        private void Speed(BossPathWalker bossPathWalker)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Walker Speed");
            m_speedText = GUILayout.TextField(bossPathWalker.m_speed.ToString());
            if (GUILayout.Button("Apply"))
            {
                float speed;

                if (float.TryParse(m_speedText, out speed))
                {
                    bossPathWalker.m_speed = speed;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void Acceleration(BossPathWalker bossPathWalker)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Walker acceleration");
            m_accelerationText = GUILayout.TextField(m_accelerationText);
            if (GUILayout.Button("Apply"))
            {
                float acceleration;

                if (float.TryParse(m_accelerationText, out acceleration))
                {
                    bossPathWalker.m_speed = acceleration;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void Update()
        {
            if(Debug.isDebugBuild)
            {
                if(Input.GetKeyDown(KeyCode.P))
                {
                    StartCoroutine(BossManual());
                }
            }
        }

        private IEnumerator BossManual()
        {
            enabled = false;
            yield return null;

            BossPathWalker bossPathWalker = GameplayPlayerManager.Instance.Boss.GetComponent<BossPathWalker>();

            float startingWalkerSpeed = bossPathWalker.m_speed;
            float startingWalkerAcceleration = bossPathWalker.m_acceleration;
            
            bossPathWalker.m_speed = 0;
            bossPathWalker.m_acceleration = 0;

            GameplayPlayerManager.Instance.DebugClearAllRunners();

            InputSource horizontalInputSource = new InputSource(Axis.LEFT_HORIZONTAL, InputDeviceManager.Instance.DefaultInputDevice);
            while(!Input.GetKeyDown(KeyCode.P))
            {
                bossPathWalker.m_position += horizontalInputSource.Value() * m_manualSpeed * Time.deltaTime;
                yield return null;
            }
            
            bossPathWalker.m_speed = startingWalkerSpeed;
            bossPathWalker.m_acceleration = startingWalkerAcceleration;

            GameplayPlayerManager.Instance.DebugActivateAllRunners();

            enabled = true;
        }
    }
}