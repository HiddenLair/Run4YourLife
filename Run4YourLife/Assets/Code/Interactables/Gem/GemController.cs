using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Player.Runner;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Interactables
{

    public class GemController : MonoBehaviour
    {

        [SerializeField]
        private float m_timeBetweenTeleports;

        [SerializeField]
        private float m_teleportDuration;

        [SerializeField]
        private FXReceiver m_spawnReceiver;

        [SerializeField]
        private FXReceiver m_destructionReceiver;

        [SerializeField]
        private FXReceiver m_teleportReceiver;

        [SerializeField]
        private GameObject m_graphicsChild;

        private Collider m_collider;

        private Transform[] m_possibleGemPositions;

        private bool m_active;

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }

        public void Init(Transform[] possibleGemPositions)
        {
            m_possibleGemPositions = possibleGemPositions;
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void ResetState()
        {
            Display(false);
            m_active = true;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void StartMoving()
        {
            StartCoroutine(GemBehaviour());
        }

        private IEnumerator GemBehaviour()
        {
            //Start by placing immediately the gem somewhere
            transform.position = RandomDifferentTeleportPosition();
            Display(true);
            m_spawnReceiver.PlayFx(false);

            //Move the gem around at intervals
            while (true)
            {
                //we let the gem still for a while
                yield return new WaitForSeconds(m_timeBetweenTeleports);

                //teleport gem
                //we hide it for some time
                m_teleportReceiver.PlayFx(false);
                Display(false);
                yield return new WaitForSeconds(m_teleportDuration);

                //gem appears again
                transform.position = RandomDifferentTeleportPosition();
                Display(true);
                m_spawnReceiver.PlayFx(false);
            }
        }

        private void Display(bool state)
        {
            m_graphicsChild.SetActive(state);
            m_collider.enabled = state;
        }

        private Vector3 RandomDifferentTeleportPosition()
        {
            Vector3 randomPosition;
            do
            {
                randomPosition = m_possibleGemPositions[UnityEngine.Random.Range(0, m_possibleGemPositions.Length)].position;
            } while (randomPosition == transform.position);
            return randomPosition;
        }

        private void Break()
        {
            m_destructionReceiver.PlayFx();
            BossFightGemManager.Instance.OnGemHasBeenDestroyed();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_active && other.CompareTag(Tags.Runner))
            {
                m_active = false;
                Break();
            }
        }
    }
}