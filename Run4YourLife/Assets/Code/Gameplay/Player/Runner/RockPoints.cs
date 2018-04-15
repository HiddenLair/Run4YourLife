using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife.Player;
using UnityEngine.EventSystems;

public class RockPoints : MonoBehaviour {


    [SerializeField]
    private float points;
    private PlayerDefinition playerWhoThrew;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.Boss)
        {
            ExecuteEvents.Execute<IScoreEvents>(other.gameObject, null, (x, y) => x.OnAddPoints(playerWhoThrew,points));
        }
    }

    public void SetPlayerDefinition(PlayerDefinition def)
    {
        playerWhoThrew = def;
    }
}
