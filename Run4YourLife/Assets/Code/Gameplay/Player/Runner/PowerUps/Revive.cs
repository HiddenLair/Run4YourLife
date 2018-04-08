using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

public class Revive : MonoBehaviour {

    private GameplayPlayerManager m_gameplayPlayerManager;

    private void Awake()
    {
        m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ExecuteEvents.Execute<IGameplayPlayerEvents>(m_gameplayPlayerManager.gameObject, null, (x, y) => x.OnPlayerReviveRequest(transform.position));
        Destroy(gameObject);
    }
}
