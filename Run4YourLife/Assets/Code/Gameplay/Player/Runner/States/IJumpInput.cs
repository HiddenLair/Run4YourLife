public interface IJumpInput
{
    int GetPriority();

    void ModifyJumpInput(ref bool input);
    void Destroy();
}
