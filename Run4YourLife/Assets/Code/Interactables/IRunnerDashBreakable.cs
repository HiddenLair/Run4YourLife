namespace Run4YourLife.Player.Runner
{
    public enum RunnerDashBreakableState
    {
        Alive,
        Broken
    }

    public interface IRunnerDashBreakable
    {
        //Can only be called if RunnerDashBreakableState == RunnerDashBreakableState.Alive
        void Break();

        RunnerDashBreakableState RunnerDashBreakableState { get; }
    }
}
