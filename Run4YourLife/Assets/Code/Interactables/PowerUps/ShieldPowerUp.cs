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
            Shielded shielded = runner.GetComponent<Shielded>();
            if(shielded == null)
            {
                shielded = runner.AddComponent<Shielded>();
            }

            shielded.SetDuration(m_duration);
        }
    }
}
