using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.GameManagement.Refactor
{
    public interface IBossDestructibleNotified
    {
        void OnDestroyed();
        void OnRegenerated();
    }
}
