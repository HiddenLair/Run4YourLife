using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public class ShieldPowerUp : PowerUp
    {
        protected override PowerUpType Type { get { return PowerUpType.Single; } }

        public override void Apply(GameObject runner)
        {
            runner.AddComponent<Shielded>();
        }
    }
}
