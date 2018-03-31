using Run4YourLife.Player;

public class Shielded : RunnerState {

    private const float END_TIME = 5.0f;

    protected override void Apply()
    {
        GetComponent<BuffManager>().ActivateShield();
        Destroy(this, END_TIME);
    }

    protected override void Unapply()
    {
        GetComponent<BuffManager>().DeactivateShield();
    }
}
