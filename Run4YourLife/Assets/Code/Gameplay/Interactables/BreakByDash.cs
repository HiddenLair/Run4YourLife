﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;
using UnityEngine.Events;

namespace Run4YourLife.Interactables
{
    public class BreakByDash : MonoBehaviour
    {
        [SerializeField]
        private GameObject destroyParticles;

        [SerializeField]
        private UnityEvent m_onBrokenByDash;


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing())
                {
                    Break();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing())
                {
                    Break();
                }
            }
        }

        private void Break()
        {
            m_onBrokenByDash.Invoke();
            Instantiate(destroyParticles, transform.position, destroyParticles.transform.rotation);
            gameObject.SetActive(false);
        }
    }
}