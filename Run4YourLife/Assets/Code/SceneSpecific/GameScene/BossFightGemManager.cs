﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Run4YourLife.GameManagement;
using Run4YourLife.SceneSpecific;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;

public class BossFightGemManager : SingletonMonoBehaviour<BossFightGemManager>
{
    [SerializeField]
    private float m_startingMinigameDelay;

    [SerializeField]
    private float m_timeBetweenGems;

    [SerializeField]
    private GameObject m_gemPrefab;

    [SerializeField]
    private Transform[] m_possibleGemPositions;

    [SerializeField]
    private GemColumnController[] m_gemColumns;

    private GameObject m_gemInstance;
    private GemController m_gemInstanceController;
    private Coroutine m_gemBehaviour;

    private int m_currentGemIndex;

    private BossFightPhaseManager m_bossFightPhaseManager;

    private void Awake()
    {
        m_bossFightPhaseManager = GetComponent<BossFightPhaseManager>();
        foreach (GemColumnController column in m_gemColumns)
        {
            column.DeactivateColumn();
        }
    }

    private void Start()
    {
        m_gemInstance = DynamicObjectsManager.Instance.GameObjectPool.Get(m_gemPrefab);
        m_gemInstanceController = m_gemInstance.GetComponent<GemController>();
        m_gemInstanceController.Init(m_possibleGemPositions);
    }

    public void StartGemMinigame()
    {
        ResetGemMinigameState();
        StartCoroutine(PlaceGemAfterDelay(m_startingMinigameDelay));
    }

    private void ResetGemMinigameState()
    {
        StopAllCoroutines();
        m_gemInstance.SetActive(false);
        m_currentGemIndex = 0;
        foreach (GemColumnController column in m_gemColumns)
        {
            column.DeactivateColumn();
        }
    }

    public void OnGemHasBeenDestroyed()
    {
        m_gemInstance.SetActive(false);

        m_gemColumns[m_currentGemIndex].ActivateColumn();
        m_currentGemIndex++;

        if (m_currentGemIndex < m_gemColumns.Length)
        {
            StartCoroutine(PlaceGemAfterDelay(m_timeBetweenGems));
        }
        else
        {
            m_bossFightPhaseManager.StartNextPhase();
        }
    }

    private IEnumerator PlaceGemAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_gemInstance.SetActive(true);
        m_gemInstanceController.StartMoving();
    }
}
