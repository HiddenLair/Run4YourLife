using System.Collections;
using System.Collections.Generic;
using Run4YourLife.Player;
using UnityEngine;

public abstract class BounceableEntityBase : MonoBehaviour, IBounceable
{
    public abstract Vector3 BounceForce { get; }

    public abstract Vector3 GetStartingBouncePosition(RunnerCharacterController runnerCharacterController);
    public abstract bool ShouldBounceByContact(RunnerCharacterController runnerCharacterController);
    public abstract void BouncedOn();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + BounceForce);
    }
}
