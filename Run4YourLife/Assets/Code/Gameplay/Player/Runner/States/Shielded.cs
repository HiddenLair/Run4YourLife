using Run4YourLife.Player;

public class Shielded : RunnerState {

    private const float END_TIME = 5.0f;

    public Shielded() : base(State.Shielded)
    {
    }

    protected override void Apply()
    {
        GetComponent<BuffManager>().SubscribeBuff(this);
        GetComponent<BuffManager>().ActivateShield();
        Destroy(this, END_TIME);
    }

    protected override void Unapply()
    {
        GetComponent<BuffManager>().UnsubscribeBuff(this);
        GetComponent<BuffManager>().DeactivateShield();
    }
}
