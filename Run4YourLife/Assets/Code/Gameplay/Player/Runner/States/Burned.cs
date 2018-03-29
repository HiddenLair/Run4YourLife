public class Burned : RunnerState, IRunnerInput
{
    private const float END_TIME = 5.0f;
    private const float SPEED_BUFF_PERCENT = 1.0f / 3.0f;

    #region Variables

    StatModifier modifier;

    #endregion

    private float lastInputSign = 1.0f;

    int IRunnerInput.GetPriority()
    {
        return 0;
    }

    public void ModifyHorizontalInput(ref float input)
    {
        float inputSign = lastInputSign;

        if(input > 0.0f)
        {
            inputSign = 1.0f;
        }
        else if(input < 0.0f)
        {
            inputSign = -1.0f;
        }

        if(inputSign != lastInputSign)
        {
            input = inputSign;
            lastInputSign = inputSign;
        }
        else
        {
            input = lastInputSign;
        }
    }

    protected override void Apply()
    {
        Destroy(this, END_TIME);
        modifier = new StatModifier(StatType.SPEED, ModifierType.PERCENT, true, SPEED_BUFF_PERCENT, END_TIME);
        GetComponent<Stats>().AddModifier(modifier);
    }

    protected override void Unapply()
    {
        GetComponent<Stats>().RemoveStatModifier(modifier);
    }
}