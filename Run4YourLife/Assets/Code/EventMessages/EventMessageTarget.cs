﻿using UnityEngine;
using UnityEngine.EventSystems;

public interface IEventMessageTarget : IEventSystemHandler
{
    void Explosion();
    void Impulse(Vector3 force);
    void Root(int rootHardness);
}
