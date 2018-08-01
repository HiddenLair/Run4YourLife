using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Run4YourLife.GameManagement;
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
    private GameObject[] m_standGems;

    private GameObject m_gemInstance;
    private GemController m_gemInstanceController;
    private Coroutine m_gemBehaviour;

    private int m_currentGemIndex;

    private BossFightPhaseManager m_bossFightPhaseManager;

    private void Awake()
    {
        m_bossFightPhaseManager = GetComponent<BossFightPhaseManager>();

        foreach (GameObject standGem in m_standGems)
        {
            standGem.SetActive(false);
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
        foreach (GameObject standGem in m_standGems)
        {
            standGem.SetActive(false);
        }
    }

    public void OnGemHasBeenDestroyed()
    {
        m_gemInstance.SetActive(false);

        m_standGems[m_currentGemIndex].SetActive(true);
        m_currentGemIndex++;

        if (m_currentGemIndex < m_standGems.Length)
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
