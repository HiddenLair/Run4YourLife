public interface IRunnerInput
{
    int GetPriority();

    void ModifyHorizontalInput(ref float input);
}