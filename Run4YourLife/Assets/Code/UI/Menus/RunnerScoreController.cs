using UnityEngine;
using UnityEngine.Playables;

using TMPro;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

public class RunnerScoreController : MonoBehaviour
{
    private PlayableDirector m_onScoreChangedPlayableDirector;
    private TextMeshProUGUI m_scoreText;
    private PlayerHandle m_playerHandle;

    private void Awake()
    {
        m_onScoreChangedPlayableDirector = GetComponent<PlayableDirector>();
        Debug.Assert(m_onScoreChangedPlayableDirector != null);

        m_scoreText = GetComponent<TextMeshProUGUI>();

        SetScore(0);
    }

    private void OnEnable()
    {
        ScoreManager.Instance.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
    }

    private void OnDisable()
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
        m_scoreText.text = ((int)Mathf.Round(score)).ToString();
        m_onScoreChangedPlayableDirector.Play();
    }
}
