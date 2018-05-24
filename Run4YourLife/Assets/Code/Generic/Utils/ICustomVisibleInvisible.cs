using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public interface ICustomVisibleInvisible : IEventSystemHandler
    {
        void OnCustomBecameInvisible();
        void OnCustomBecameVisible();
    }
}