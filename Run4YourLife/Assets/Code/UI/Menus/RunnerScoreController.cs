using TMPro;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

public class RunnerScoreController : MonoBehaviour
{
    private ScaleTick m_scaleTick;
    private ScoreManager m_scoreManager;
    private TextMeshProUGUI m_scoreText;
    private PlayerHandle m_playerHandle;
    private float m_score;

    private void Awake()
    {
        m_scaleTick = GetComponent<ScaleTick>();
        m_scoreText = GetComponent<TextMeshProUGUI>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_scoreManager.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);

        SetScore(0);
    }

    private void OnDestroy()
    {
        m_scoreManager.OnPlayerScoreChanged.RemoveListener(OnPlayerScoreChanged);
    }

    public void SetPlayerDefinition(PlayerHandle playerDefinition)
    {
        m_playerHandle = playerDefinition;
    }

    private void OnPlayerScoreChanged(PlayerHandle playerHandle, float score)
    {
        if(playerHandle == m_playerHandle)
        {
            SetScore(score);
        }
    }

    private void SetScore(float score)
    {
        m_scaleTick.Tick();
        m_score = score;
        m_scoreText.text = ((int)Mathf.Round(score)).ToString();
    }
}
