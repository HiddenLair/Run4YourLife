﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Run4YourLife.Player;

public class RunnersScoreController : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreTextPrefab;

    private HorizontalLayoutGroup scoreTexts;

    void Awake()
    {
        scoreTexts = GetComponent<HorizontalLayoutGroup>();
    }

    void Start()
    {
        foreach (PlayerHandle playerHandle in PlayerManager.Instance.RunnerPlayerHandles)
        {
            GameObject scoreText = Instantiate(scoreTextPrefab, scoreTexts.transform, false);
            scoreText.GetComponent<RunnerScoreController>().SetplayerHandle(playerHandle);
        }
    }
}