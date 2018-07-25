using UnityEngine;
using Run4YourLife.Player.Runner;

namespace Run4YourLife.Interactables
{
    public interface IBounceable
    {

        Vector3 BounceForce { get; }
        void BouncedOn();
        bool ShouldBounceByContact(RunnerController runnerCharacterController);
        Vector3 GetStartingBouncePosition(RunnerController runnerCharacterController);
    }
}
