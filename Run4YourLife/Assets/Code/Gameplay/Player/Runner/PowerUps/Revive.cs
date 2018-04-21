using Run4YourLife.Player;
using UnityEngine.EventSystems;
using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife;


public class Revive : MonoBehaviour {

    [SerializeField]
    private float points;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Runner))
        {
            PlayerHandle playerHandle = other.gameObject.GetComponent<PlayerInstance>().PlayerHandle;
            ExecuteEvents.Execute<IScoreEvents>(ScoreManager.InstanceGameObject, null, (x, y) => x.OnAddPoints(playerHandle, points));
            ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerReviveRequest(transform.position));
            Destroy(gameObject);
        }
    }
}
