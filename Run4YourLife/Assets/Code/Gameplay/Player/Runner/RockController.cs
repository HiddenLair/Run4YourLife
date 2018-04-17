using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife.Player;
using UnityEngine.EventSystems;

using Run4YourLife;

public class RockController : MonoBehaviour {

    [SerializeField]
    private float points;
    private PlayerDefinition playerWhoThrew;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Boss))
        {
            ExecuteEvents.Execute<IScoreEvents>(FindObjectOfType<ScoreManager>().gameObject, null, (x, y) => x.OnAddPoints(playerWhoThrew, points));
        }
    }

    public void SetPlayerDefinition(PlayerDefinition def)
    {
        playerWhoThrew = def;
    }
}
