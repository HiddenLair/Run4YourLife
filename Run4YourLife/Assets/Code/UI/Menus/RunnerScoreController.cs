using UnityEngine;
using UnityEngine.Playables;

using TMPro;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

public class RunnerScoreController : MonoBehaviour
{
    private PlayableDirector m_onScoreChangedPlayableDirector;
    private TextMeshProUGUI m_scoreText;

    private void Awake()
    {
        m_onScoreChangedPlayableDirector = GetComponent<PlayableDirector>();
        Debug.Assert(m_onScoreChangedPlayableDirector != null);

        m_scoreText = GetComponent<TextMeshProUGUI>();

        SetScore(0);
    }

    public void SetScore(float score)
    {
        m_scoreText.text = ((int)Mathf.Round(score)).ToString();
        m_onScoreChangedPlayableDirector.Play();
    }
}