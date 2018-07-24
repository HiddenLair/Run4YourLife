using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.GameManagement.Refactor
{
    public enum BossDestructibleState
    {
        Alive,
        InDestruction,
        Destroyed
    }

    public interface IBossDestructible
    {
        void Destroy();
        void Regenerate();

        float DestroyPosition { get; }
        BossDestructibleState BossDestructibleState { get; }
    }
}