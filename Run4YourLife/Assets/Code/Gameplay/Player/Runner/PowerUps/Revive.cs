using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

public class Revive : MonoBehaviour {

    #region Variables

    GameplayPlayerManager playerManager;

    #endregion

    private void Awake()
    {
        playerManager = FindObjectOfType<GameplayPlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ExecuteEvents.Execute<IGameplayPlayerEvents>(playerManager.gameObject, null, (x, y) => x.OnPlayerReviveRequest(transform.position));
        Destroy(gameObject);
    }
}
