using TMPro;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

public class RunnerScoreController : MonoBehaviour
{
    private ScaleTick scaleTick;
    private ScoreManager scoreManager;
    private TextMeshProUGUI scoreText;
    private PlayerDefinition playerDefinitionTarget;

    private float previousPoints = 0.0f;

    void Awake()
    {
        scaleTick = GetComponent<ScaleTick>();
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }
	
    void Start()
    {
        float points = scoreManager.GetPointsByPlayerDefinition(playerDefinitionTarget);
        scoreText.text = ((int)Mathf.Round(points)).ToString();
    }

	void Update()
    {
		if(playerDefinitionTarget != null)
        {
            float points = scoreManager.GetPointsByPlayerDefinition(playerDefinitionTarget);

            if(points != previousPoints)
            {
                scaleTick.Tick();
                previousPoints = points;
                scoreText.text = ((int)Mathf.Round(points)).ToString();
            }
        }
	}

    public void SetPlayerDefinition(PlayerDefinition playerDefinition)
    {
        playerDefinitionTarget = playerDefinition;
    }
}
