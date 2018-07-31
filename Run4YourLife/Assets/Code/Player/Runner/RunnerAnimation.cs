using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerAnimation : MonoBehaviour
{

    public class States
    {
        public static readonly string Idle = "Idle";
        public static readonly string Move = "Move";
        public static readonly string Airborne = "Airborne";
        public static readonly string Frontflip = "Frontflip";
        public static readonly string Push = "Push";
        public static readonly string Dash = "Dash";
        public static readonly string Shock = "Shock";
        public static readonly string Dance = "Dance";
    }

    public class Parameters
    {
        public class Float
        {
            public static readonly string HorizontalSpeed = "xSpeed";
            public static readonly string VerticalSpeed = "ySpeed";
        }

        public class Bool
        {
            public static readonly string Idle = "Idle";
            public static readonly string Move = "Move";
            public static readonly string Airborne = "Airborne";
        }

        public class Triggers
        {
            public static readonly string Frontflip = "Frontflip";
            public static readonly string Push = "Push";
            public static readonly string Dash = "Dash";
            public static readonly string Shock = "Shock";
            public static readonly string Dance = "Dance";
        }
    }
}
