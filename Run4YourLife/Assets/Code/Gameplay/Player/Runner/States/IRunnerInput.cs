public interface IRunnerInput
{
    int GetPriority();

    void Apply(ref float input);
}