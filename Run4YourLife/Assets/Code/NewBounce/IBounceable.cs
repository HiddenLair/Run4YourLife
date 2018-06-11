using UnityEngine;
using Run4YourLife.Player;

public interface IBounceable {

    Vector3 BounceForce { get; }
    void BouncedOn();
    bool ShouldBounceByContact(RunnerCharacterController runnerCharacterController);
    Vector3 GetStartingBouncePosition(RunnerCharacterController runnerCharacterController);
}
