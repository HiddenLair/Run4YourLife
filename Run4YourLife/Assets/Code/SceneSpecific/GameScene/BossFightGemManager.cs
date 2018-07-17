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
    private float m_timeBetweenGems;

    [SerializeField]
    private float m_timeBetweenTeleports;

    [SerializeField]
    private float m_timeInvisibleAfterTeleport;

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

        foreach(GameObject standGem in m_standGems)
        {
            standGem.SetActive(false);
        }
    }

    private void Start()
    {
        m_gemInstance = DynamicObjectsManager.Instance.GameObjectPool.Get(m_gemPrefab);
        m_gemInstanceController = m_gemInstance.GetComponent<GemController>();
    }

    public void PlaceFirstGem()
    {
        ResetState();

        m_gemBehaviour = StartCoroutine(GemBehaviour(false));
    }

    private void ResetState()
    {
        m_gemInstance.SetActive(false);
        m_currentGemIndex = 0;
        foreach (GameObject standGem in m_standGems)
        {
            standGem.SetActive(false);
        }
    }

    public void OnGemHasBeenDestroyed()
    {
        StopCoroutine(m_gemBehaviour); 
        m_gemBehaviour = null;
        m_gemInstance.SetActive(false);

        m_standGems[m_currentGemIndex].SetActive(true);
        m_currentGemIndex++;

        if (m_currentGemIndex < m_standGems.Length)
        {
            m_gemBehaviour = StartCoroutine(GemBehaviour(true));
        }
        else
        {
            m_bossFightPhaseManager.StartNextPhase();
        }
    }

    private IEnumerator GemBehaviour(bool startingDelay)
    {
        if (startingDelay)
        {
            yield return new WaitForSeconds(m_timeBetweenGems);
        }

        m_gemInstance.SetActive(true);
        Vector3 position;
        
        //Start by placing immediately the gem somewhere
        position = RandomDifferentTeleportPosition(m_gemInstance.transform.position);
        m_gemInstanceController.TeleportToPositionAfterTime(position, 0);

        //Move the gem around at intervals
        while (true)
        {
            yield return new WaitForSeconds(m_timeBetweenTeleports);
            position = RandomDifferentTeleportPosition(m_gemInstance.transform.position);
            m_gemInstanceController.TeleportToPositionAfterTime(position, m_timeInvisibleAfterTeleport);
        }
    }

    private Vector3 RandomDifferentTeleportPosition(Vector3 position)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = m_possibleGemPositions[Random.Range(0, m_possibleGemPositions.Length)].position;
        } while (randomPosition == position);
        return randomPosition;
    }
}
