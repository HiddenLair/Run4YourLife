using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class ShieldPowerUp : PowerUp
    {
        [SerializeField]
        private int m_duration;

        protected override PowerUpType Type { get { return PowerUpType.Single; } }

        public override void Apply(GameObject runner)
        {
            runner.GetComponent<RunnerController>().ActivateShield(m_duration);
        }
    }
}
