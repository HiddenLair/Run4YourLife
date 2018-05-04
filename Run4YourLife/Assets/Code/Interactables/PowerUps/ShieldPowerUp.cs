using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class ShieldPowerUp : PowerUp
    {
        [SerializeField]
        private int shieldCooldown = 5;

        protected override PowerUpType Type { get { return PowerUpType.Single; } }

        public override void Apply(GameObject runner)
        {
            runner.GetComponent<RunnerController>().ActivateShield(shieldCooldown);
        }
    }
}
