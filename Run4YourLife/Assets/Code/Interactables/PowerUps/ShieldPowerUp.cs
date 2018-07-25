using UnityEngine;

using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables
{
    public class ShieldPowerUp : PowerUp
    {
        [SerializeField]
        private int m_duration;

        [SerializeField]
        private FXReceiver pickParticle;

        protected override PowerUpType Type { get { return PowerUpType.Shared; } }

        public override void Apply(GameObject runner)
        {
            pickParticle.PlayFx(false);

            Shielded shielded = runner.GetComponent<Shielded>();
            if (shielded == null)
            {
                shielded = runner.AddComponent<Shielded>();
            }

            shielded.SetDuration(m_duration);
        }
    }
}
