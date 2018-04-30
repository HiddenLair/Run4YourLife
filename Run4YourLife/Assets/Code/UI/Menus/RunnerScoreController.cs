using TMPro;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

public class RunnerScoreController : MonoBehaviour
{
    private ScaleTick m_scaleTick;
    private TextMeshProUGUI m_scoreText;
    private PlayerHandle m_playerHandle;

    private void Awake()
    {
        m_scaleTick = GetComponent<ScaleTick>();
        m_scoreText = GetComponent<TextMeshProUGUI>();

        SetScore(0);
    }

    private void Start()
    {
        ScoreManager.Instance.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnPlayerScoreChanged.RemoveListener(OnPlayerScoreChanged);
    }

    public void SetplayerHandle(PlayerHandle playerHandle)
    {
        m_playerHandle = playerHandle;
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
        m_scoreText.text = ((int)Mathf.Round(score)).ToString();
    }
}
