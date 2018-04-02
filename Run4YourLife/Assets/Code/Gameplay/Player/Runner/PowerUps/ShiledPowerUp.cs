using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public class ShiledPowerUp : PowerUp
    {
        public override void Effect(GameObject g)
        {
            g.AddComponent<Shielded>();
        }
    }
}
