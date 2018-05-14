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
        private string m_positionText;

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
                    Position(bossPathWalker);
                }
            }
        }

        private void Position(BossPathWalker bossPathWalker)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Position: "+bossPathWalker.m_position);
            m_positionText = GUILayout.TextField(m_positionText);
            if (GUILayout.Button("Apply"))
            {
                float position;

                if (float.TryParse(m_positionText, out position))
                {
                    bossPathWalker.m_position = position;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void Speed(BossPathWalker bossPathWalker)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Speed: "+bossPathWalker.m_speed);
            m_speedText = GUILayout.TextField(m_speedText);
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

            GUILayout.Label("Acceleration: "+bossPathWalker.m_acceleration);
            m_accelerationText = GUILayout.TextField(m_accelerationText);
            if (GUILayout.Button("Apply"))
            {
                float acceleration;

                if (float.TryParse(m_accelerationText, out acceleration))
                {
                    bossPathWalker.m_acceleration = acceleration;
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

                if(Input.GetKeyDown(KeyCode.O))
                {
                    PauseResumeWalker();
                }
            }
        }

        private void PauseResumeWalker()
        {
            BossPathWalker bossPathWalker = GameplayPlayerManager.Instance.Boss.GetComponent<BossPathWalker>();
            bossPathWalker.enabled = !bossPathWalker.enabled;
        }

        private IEnumerator BossManual()
        {
            enabled = false;

            BossPathWalker bossPathWalker = GameplayPlayerManager.Instance.Boss.GetComponent<BossPathWalker>();

            if(bossPathWalker != null)
            {
                yield return null; // P Key down is true, wait one frame so that it does not skip the while
                float startingWalkerSpeed = bossPathWalker.m_speed;
                float startingWalkerAcceleration = bossPathWalker.m_acceleration;
                
                bossPathWalker.m_speed = 0;
                bossPathWalker.m_acceleration = 0;

                GameplayPlayerManager.Instance.DebugClearAllRunners();

                InputSource horizontalInputSource = new InputSource(Axis.LEFT_HORIZONTAL, InputDeviceManager.Instance.DefaultInputDevice);
                while(!Input.GetKeyDown(KeyCode.P))
                {
                    float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2.0f : 1.0f;
                    bossPathWalker.m_position += horizontalInputSource.Value() * speedMultiplier * m_manualSpeed * Time.deltaTime;
                    yield return null;
                }
                
                bossPathWalker.m_speed = startingWalkerSpeed;
                bossPathWalker.m_acceleration = startingWalkerAcceleration;

                GameplayPlayerManager.Instance.DebugActivateAllRunners();
            }

            enabled = true;
        }
    }
}