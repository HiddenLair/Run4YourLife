using System;

public class Root : RunnerState, IInteractInput
{
    #region Variables

    private int remainingHits = 4;

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
        modifierSpeed = new SpeedModifier(ModifierType.SETTER, true, 0, -1);
        modifierJump = new JumpHeightModifier(ModifierType.SETTER, true, 0, -1); // TODO absorb also this input
        GetComponent<Stats>().AddModifier(modifierSpeed);
        GetComponent<Stats>().AddModifier(modifierJump);
    }

    protected override void Unapply()
    {
        GetComponent<Stats>().RemoveStatModifier(modifierSpeed);
        GetComponent<Stats>().RemoveStatModifier(modifierJump);
    }

    public void SetHardness(int rootHardness)
    {
        if (rootHardness > 0)
        {
            remainingHits = rootHardness;
        }
    }
}
