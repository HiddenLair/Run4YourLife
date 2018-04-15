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
        Debug.Assert(m_gameplayPlayerManager);
    }

    private void OnTriggerEnter(Collider other)
    {
        ExecuteEvents.Execute<IGameplayPlayerEvents>(m_gameplayPlayerManager.gameObject, null, (x, y) => x.OnRunnerReviveRequest(transform.position));
        Destroy(gameObject);
    }
}
