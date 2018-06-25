using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class BossAnimation {

        public class StateNames {
            public static readonly string Move = "Move";
            public static readonly string Shoot = "Shoot";
            public static readonly string Laugh = "Laugh";
            public static readonly string Mele = "Mele";
            public static readonly string Cast = "Cast";
            public static readonly string ThreateningScream = "Threatening Scream";
        }

        public class Triggers {
            public static readonly string Shoot = "Shoot";
            public static readonly string Mele = "Mele";
            public static readonly string Cast = "Casting";
            public static readonly string Laugh = "Laugh";
        }
    }
}
