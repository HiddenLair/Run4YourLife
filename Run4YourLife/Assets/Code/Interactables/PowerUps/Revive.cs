using UnityEngine.EventSystems;
using UnityEngine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Interactables
{
    public class Revive : PowerUp
    {
        protected override PowerUpType Type { get { return PowerUpType.Void; } }

        public override void Apply(GameObject runner)
        {
            Debug.Log("Legacy, maybe remove"); // TODO
            //ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnReviveRunnerRequest(transform.position));
        }
    }
}