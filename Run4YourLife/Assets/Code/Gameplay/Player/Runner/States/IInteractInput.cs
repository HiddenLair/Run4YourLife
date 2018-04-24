public interface IInteractInput
{
    int GetPriority();

    void ModifyInteractInput(ref bool input);
    void Destroy();
}