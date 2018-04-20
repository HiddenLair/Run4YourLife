using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife.Player;
using UnityEngine.EventSystems;

using Run4YourLife;

public class RockController : MonoBehaviour {

    [SerializeField]
    private float points;

    private PlayerHandle playerWhoThrew;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.BossRockScore))
        {
            ExecuteEvents.Execute<IScoreEvents>(GameObject.FindObjectOfType<ScoreManager>().gameObject, null, (x, y) => x.OnAddPoints(playerWhoThrew, points));
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetPlayerDefinition(PlayerHandle def)
    {
        playerWhoThrew = def;
    }
}
