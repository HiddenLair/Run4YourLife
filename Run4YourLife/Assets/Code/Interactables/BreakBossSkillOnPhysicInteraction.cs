﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Interactables;

public class BreakBossSkillOnPhysicInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (gameObject.activeInHierarchy)
        {
            CheckBossSkillBreakableAndSendBreakEvent(collider.gameObject);
        }
    }

    private void CheckBossSkillBreakableAndSendBreakEvent(GameObject gameObject)
    {
        IBossSkillBreakable bossSkillBreakable = gameObject.GetComponent<IBossSkillBreakable>();
        if (bossSkillBreakable != null)
        {
            bossSkillBreakable.Break();
        }
    }
}