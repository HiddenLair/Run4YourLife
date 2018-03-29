using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;

using Run4YourLife.Utils;

namespace Run4YourLife.Player
{
    public class BigHeadPowerUp : PowerUp
    {
        public override void Effect(GameObject g)
        {
            g.AddComponent<BigHead>();
        }
    }
}