﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : RunnerState,IInteractInput {

    private const int HITS = 4;

    #region Variables

    int remainingHits = HITS;

    StatModifier modifierSpeed;
    StatModifier modifierJump;

    #endregion

    int IInteractInput.GetPriority()
    {
        return 1;
    }

    public void ModifyInteractInput(ref bool input)
    {
        if (input)
        {
            if(--remainingHits == 0)
            {
                Destroy(this);
            }
        }

        input = false;
    }

    protected override void Apply()
    {
        modifierSpeed = new StatModifier(StatType.SPEED, ModifierType.SETTER, true, 0, -1);
        modifierJump = new StatModifier(StatType.JUMP_HEIGHT, ModifierType.SETTER, true, 0, -1);//TODO absorb also this input
        GetComponent<Stats>().AddModifier(modifierSpeed);
        GetComponent<Stats>().AddModifier(modifierJump);
    }

    protected override void Unapply()
    {
        GetComponent<Stats>().RemoveStatModifier(modifierSpeed);
        GetComponent<Stats>().RemoveStatModifier(modifierJump);
    }
}
