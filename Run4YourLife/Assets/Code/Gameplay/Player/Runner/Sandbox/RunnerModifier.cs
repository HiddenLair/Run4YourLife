using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public enum ModifierType
    {
        HorizontalInput,
        JumpInput,
        InteractInput,
        Buff
    }

    public abstract class RunnerModifier : ScriptableObject
    {
        public abstract ModifierType ModifierType { get; }
        public abstract void Apply(); // aqui falta pasarle el objeto del jugador
        public abstract void Cease(); // aqui falta pasarle el objeto del jugador
    }
}