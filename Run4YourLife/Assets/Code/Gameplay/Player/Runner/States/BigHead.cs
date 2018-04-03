using Run4YourLife.Player;

public class BigHead : RunnerState {

    private const float END_TIME = 5.0f;
    private const float BOUNCE_BUFF_PERCENT = 2.0f;

    #region Variables

    StatModifier modifier;

    #endregion

    public BigHead():base(State.BigHead)
    {
    }

    protected override void Apply()
    {
        GetComponent<BuffManager>().SubscribeBuff(this);
        GetComponent<BuffManager>().IncreaseHeadPercentual(BOUNCE_BUFF_PERCENT);
        Destroy(this, END_TIME);
        modifier = new BounceHeightModifier(ModifierType.PERCENT, true, BOUNCE_BUFF_PERCENT, END_TIME);
        GetComponent<Stats>().AddModifier(modifier);
    }

    protected override void Unapply()
    {
        GetComponent<BuffManager>().UnsubscribeBuff(this);
        GetComponent<BuffManager>().DecreaseHeadPercentual(BOUNCE_BUFF_PERCENT);
        GetComponent<Stats>().RemoveStatModifier(modifier);
    }
}
