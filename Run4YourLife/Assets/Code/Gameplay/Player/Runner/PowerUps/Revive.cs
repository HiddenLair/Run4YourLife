using Run4YourLife.Player;
using UnityEngine.EventSystems;
using UnityEngine;
using Run4YourLife.GameManagement;

public class Revive : MonoBehaviour {

    [SerializeField]
    private float points;
    private GameplayPlayerManager m_gameplayPlayerManager;

    private void Awake()
    {
        m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
        Debug.Assert(m_gameplayPlayerManager);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHandle player = other.gameObject.GetComponent<PlayerInstance>().PlayerHandle;
        ExecuteEvents.Execute<IScoreEvents>(FindObjectOfType<ScoreManager>().gameObject, null, (x, y) => x.OnAddPoints(player, points));
        ExecuteEvents.Execute<IGameplayPlayerEvents>(m_gameplayPlayerManager.gameObject, null, (x, y) => x.OnRunnerReviveRequest(transform.position));
        Destroy(gameObject);
    }
}
