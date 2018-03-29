public class WindLeft : RunnerState,IRunnerInput {

    private const float WIND_FORCE = 0.7f;

    int IRunnerInput.GetPriority()
    {
        return 0;
    }

    public void ModifyHorizontalInput(ref float input)
    {
        input -= WIND_FORCE;
    }
}
