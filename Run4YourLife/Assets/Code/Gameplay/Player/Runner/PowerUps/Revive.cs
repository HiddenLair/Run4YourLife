using Run4YourLife.Player;
using UnityEngine.EventSystems;
using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife;

namespace Run4YourLife.Player
{
    public class Revive : PowerUp
    {
        protected override PowerUpType Type { get { return PowerUpType.Void; } }

        public override void Apply(GameObject runner)
        {
            ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerReviveRequest(transform.position));
        }
    }
}