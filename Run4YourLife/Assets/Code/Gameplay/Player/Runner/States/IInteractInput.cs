public interface IInteractInput
{
    int GetPriority();

    void ModifyInteractInput(ref bool input);
}